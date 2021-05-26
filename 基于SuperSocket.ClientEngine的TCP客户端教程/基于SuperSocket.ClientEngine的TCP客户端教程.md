1. 为项目添加两个dll，分别是SuperSocket.ClientEngine.dll和SuperSocket.ProtoBase.dll
2. 添加命名空间 using SuperSocket.ClientEngine和using SuperSocket.ProtoBase



初始化（连接，或打开）

开启接收线程，线程从接收缓存拿收到的字节，解决粘包提取出一帧完整的命令消息（字节格式），然后将字节格式的消息帧转换成类（消息模型）

关闭

```csharp
class StandardCommunicationClient
{
    public void Initialize();
    
    public void Send();
    
    private void StartReciveThread()
    {
       byte[] rawMsg = FetchBytesFromRecivedBuffer();
       byte[] frame = ResolveSingleFrame(rawMsg);
       IpackageInfo packageInfo = Parse(frame);
       HandlePackage(packageInfo);
    }
    
    public void Release();
}
```

1. ```Initialize``` 

   ​		初始化。举例：TCP客户端指连接，分配内存用以存储和初始化TCP协议中一些状态，如序列号等信息。串口指设置串口参数打开串口占用串口。

2. ```Send``` 

   ​		发送消息。

3. ```StartReciveThread``` 

   ​		接收消息线程。当消息到达目标主机后，硬件层触发上层的接收消息的API，从接收缓存获取字节消息，然后根据协议特点按照既定的解决粘包方案提取出一帧完整的字节格式的命令消息体，再将该字节格式的消息体转换成抽象交互协议类的实例，最终再触发命令对应的事件。

4. ```Release``` 

   ​			释放资源。正确释放资源，优雅的结束通信相关的线程。

上述最复杂的是开启接收消息的线程。

第一个难点是接收消息的API是阻塞的，我们如何用最快的速度去响应消息到达硬件这一事件。这个难点，常见的通信框架都会帮我们解决，我们只需要处理剩余的其他难点。

对交互协议抽象成类，解决粘包的方法，解决将字节格式的一帧命令消息转换成类的实例，命令对应的消息处理程序，

如何传入一个方法？

接口

委托

如何定义协议包？

用接口。接口也能约束客户通过接口实现一些变量集合体。



```csharp
// 从接收缓存拿到当前接收到的字节
// 解决粘包，提取完整的一帧消息
// 转换成易于处理的协议格式
```

1. 如何传进去一个方法
   1. 接口
   2. 委托



自定义结构体







# Samples

## 需求描述

客户端请求服务器的计算服务。客户端告诉服务器运算符和运算数，服务器返回客户端计算结果。

## 步骤一：确定交互协议

### 请求格式

| Command       | Parameter1  | Parameter2 |Result|
| :------------ | :-------------: | ------------: |:-------------:|
| Add   | 1.1 |         2.2 |Double.NaN|
| Sub   |    10    |         20 |Double.NaN|
| Mul |    3.14    |          2 |Double.NaN|
| Div |    2020    |         20 |Double.NaN|

### 响应格式
| Command       | Parameter1  | Parameter2 |Result|
| :------------ | :-------------: | ------------: |:-------------:|
| Add   | 1.1 |         2.2 |3.3|
| Sub   |    10    |         20 |-10|
| Mul |    3.14    |          2 |6.28|
| Div |    2020    |         20 |101|

