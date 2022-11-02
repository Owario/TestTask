using System.Collections;
using System.ComponentModel;

namespace Test;

public class ListRandom : IEnumerable<ListNode>
    {
        private ListNode? _head;
        private ListNode? _tail;
        public int Count { get; private set; }
        
        public bool IsDataValid { get; private set; }
        
        public UpdateType UpdateType { get; set; }

        public ListRandom()
        {
            _head = null;
            _tail = null;
            Count = 0;
            IsDataValid = true;
            UpdateType = UpdateType.NewData;
        }
        
        public void PushBack(string data)
        {
            ListNode node = new ListNode(data);

            if (_head == null || _tail==null)
            {
                _head = node;
                _tail = node;
            }
            else
            {
                _tail.Next = node;
                node.Previous = _tail;
            }
            _tail = node;
            Count++;
            IsDataValid = false;
        }

        public ListNode? RemoveBack()
        {
            if (_head == null || _tail==null)
            {
                return null;
            }
            else
            {
                IsDataValid = false;
                ListNode? tmp;
                if (_head == _tail)
                {
                    tmp = _head;
                    _head = null;
                    _tail = null;
                    Count--;
                    return tmp;
                }
                foreach (var listNode in this.Where(n=>n.Random==_tail))
                {
                    listNode.Random = null;
                }

                tmp = _tail;
                _tail = _tail.Previous;
                if (_tail!=null) _tail.Next = null;
                Count--;
                return tmp;
            }
        }
        

        private ListNode? this[int index]
        {
            get
            {
                if (index > Count || index < 0)
                {
                    return null;
                }
                var currIndex = 0;
                ListNode? iter = _head;
                while (currIndex != index)
                {
                    if (currIndex == index)
                    {
                        break;
                    }

                    iter = iter?.Next;

                    currIndex++;
                }

                return iter;
            }
        }

        public void UpdateDataValid()
        {
            switch (UpdateType)
            {
                case UpdateType.AllData:
                {
                    MakeDataValid(this);
                    break;
                }
                case UpdateType.NewData:
                {
                    MakeDataValid(this
                        .Where(n => n.Random == null));
                    break;
                }
            }
        }
        
        private void MakeDataValid(IEnumerable<ListNode> collection)
        {
            var rand = new Random();
            foreach (var nodeList in collection)
            {
                int index = rand.Next(Count);
                nodeList.Random = this[index];
            }
            IsDataValid = true;
        }

        private int? GetNodeOrder(ListNode node)
        {
            int order = 0;
            foreach (var currNode in this)
            {
                if (currNode == node)
                {
                    return order;
                }

                order++;
            }

            return null;
        }
        public void Serialize(Stream s)
        {
            if (!IsDataValid)
            {
                UpdateDataValid();
            }

            
            if (s is FileStream)
            {
                using (BinaryWriter writer = new BinaryWriter(s))
                {
                    // Вписываем количество 
                    writer.Write((int)Count);
                    foreach (var node in this)
                    {
                        // Вписываем порядковый номер рандомной ноды на которую указывает текущая
                        writer.Write((int)GetNodeOrder(node.Random));
                        // Вписываем данные текущей ноды
                        writer.Write((string)node.Data);
                    }
                    Console.WriteLine("The data is serialized in a file");
                }
            }
        }


        public void Deserialize(Stream s)
        {
            Clear();
            if (s is FileStream)
            {
                using (BinaryReader reader = new BinaryReader(s))
                {
                    Count = reader.ReadInt32();
                    ListNode[] listOfNodes = new ListNode[Count];
                    for (int i = 0; i < Count; ++i)
                    {
                        listOfNodes[i] = new ListNode();
                    }
                    
                    for (int i = 0; i < Count; ++i)
                    {
                        int randomNodeOrder = reader.ReadInt32();
                        listOfNodes[i].Random = listOfNodes[randomNodeOrder];
                        string nodeData = reader.ReadString();
                        listOfNodes[i].Data = nodeData;
                        if ((i != 0) && (i != Count - 1))
                        {
                            listOfNodes[i].Previous = listOfNodes[i - 1];
                            listOfNodes[i].Next = listOfNodes[i + 1];
                        }
                    }

                    _head = listOfNodes[0];
                    _head.Next = listOfNodes[1];

                    _tail = listOfNodes[Count-1];
                    _tail.Previous = listOfNodes[Count - 2];
                    Console.WriteLine("File read, data deserialized");
                }
            }
        }


        public void PrintList()
        {
            if (_head == null || _tail == null)
            {
                return;
            }

            int counter = 0;
            string random, previous, next;
            foreach (var node in this)
            {
                random = "-";
                if (node.Random != null)
                {
                    random = node.Random.Data;
                }

                previous = "-";
                if (node.Previous != null)
                {
                    previous = node.Previous.Data;
                }

                next = "-";
                if (node.Next != null)
                {
                    next = node.Next.Data;
                }
                Console.WriteLine("{0} Нода: {4}, следующая {1}, предыдушая {2}, рандомная {3}", counter
                    , next, previous, random, node.Data);
                
                counter++;
            }

        }
        private void Clear()
        {
            _head = null;
            _tail = null;
            Count = 0;
        }
        
        public IEnumerator<ListNode> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                var a = this[i];
                if (a != null) yield return a;
            }
        }
        
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        
    }
