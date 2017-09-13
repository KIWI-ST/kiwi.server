namespace Engine.Image
{
    public class Container<T>
    {
        private T[] array;
        //索引大小
        private int _count;

        public Container(int number)
        {
            array = new T[number];
            _count = number;

        }
        /// <summary>
        /// 索引总长度
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        public T this[int i]
        {
            get
            {
                return array[i];
            }
            set
            {
                array[i] = value;
            }
        }
    }
}
