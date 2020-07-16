# ExpressionMove

两个相似类型的 Expression 表达式的转换；如：领域模型和数据库类型的表达式转换。

### 特点

可以轻松的将一个 Expression<Func<T1, bool>> 转换为 Expression<Func<T2, bool>>。

### 安装程序包：

dotnet add package xLiAd.ExpressionMove

### 使用方法：

假设你有以下两个类：

```csharp
public class Model1
{
	public int Id { get; set; }
	public string Name { get; set; }
}
public class Model2
{
	public int Id { get; set; }
	public string Name { get; set; }
}
```
现在你有一个 Lambda 表达式：
```csharp
Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.Name == "a";
```
当你想把 expression 转换为 Expression<Func<Model2, bool>> 时，可以这样做：

```csharp
var expression2 = expression.BuildMover().MoveTo<Model2>();
```

对于字段名称不同或类型不同的类，可参见单元测试用例的写法。