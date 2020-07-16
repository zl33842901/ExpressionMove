# ExpressionMove

�����������͵� Expression ���ʽ��ת�����磺����ģ�ͺ����ݿ����͵ı��ʽת����

### �ص�

�������ɵĽ�һ�� Expression<Func<T1, bool>> ת��Ϊ Expression<Func<T2, bool>>��

### ��װ�������

dotnet add package xLiAd.ExpressionMove

### ʹ�÷�����

�����������������ࣺ

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
��������һ�� Lambda ���ʽ��
```csharp
Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.Name == "a";
```
������� expression ת��Ϊ Expression<Func<Model2, bool>> ʱ��������������

```csharp
var expression2 = expression.BuildMover().MoveTo<Model2>();
```

�����ֶ����Ʋ�ͬ�����Ͳ�ͬ���࣬�ɲμ���Ԫ����������д����