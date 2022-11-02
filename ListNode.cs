namespace Test;

public class ListNode
{
    public ListNode(string data = "")
    {
        Data = data;
    }
    public ListNode? Previous { get; set; }
    public ListNode? Next { get; set; }
    public ListNode? Random { get; set; } // произвольный элемент внутри списка
    public string Data { get; set; }
}