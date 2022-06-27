namespace SimpleCache.Core;

internal class LRUItem<T, U>
    where T: notnull
{
    public LinkedListNode<T> Node { get; set; }
    public U Value { get; set; }

    public LRUItem(LinkedListNode<T> node, U value){
        this.Node = node;
        this.Value = value;
    }

    public (T key, U value) ToKeyValue(){
        return (Node.Value, Value);
    }
}
