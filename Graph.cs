namespace Lab2
{
    class Graph
    {
        private object[] vertices;
        private bool[,] edges;

        public Graph(int numVertices)
        {
            vertices = new object[numVertices];
            edges = new bool[numVertices, numVertices];
        }
        public void addVertexData(int vertexNumber, object vertexData)
        {
            if (vertexNumber < 0 || vertexNumber >= vertices.Length) // Invalid indexes
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                vertices[vertexNumber] = vertexData;
            }
        }
        public void addEdge(int vertex1, int vertex2)
        {
            if (vertex1 < 0 || vertex1 >= vertices.Length || vertex2 < 0 || vertex2 >= vertices.Length) // Invalid indexes
            {
                throw new IndexOutOfRangeException();
            }
            else if (hasEdge(vertex1, vertex2))     // Edge already exists
            {
                throw new ArgumentException();
            }
            else if (vertex1 == vertex2)    // No self loops
            {
                throw new ArgumentException();
            }
            else
            {
                edges[vertex1, vertex2] = true;
                edges[vertex2, vertex1] = true;
            }
        }
        public void removeEdge(int vertex1, int vertex2)
        {
            if (vertex1 < 0 || vertex1 >= vertices.Length || vertex2 < 0 || vertex2 >= vertices.Length) // Invalid indexes
            {
                throw new IndexOutOfRangeException();
            }
            else if (!hasEdge(vertex1, vertex2))    // Edge does not exist
            {
                throw new ArgumentException();
            }
            else
            {
                edges[vertex1, vertex2] = false;
                edges[vertex2, vertex1] = false;
            }
        }
        public bool hasEdge(int vertex1, int vertex2)
        {
            if (vertex1 < 0 || vertex1 >= vertices.Length || vertex2 < 0 || vertex2 >= vertices.Length) // Invalid indexes
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return edges[vertex1, vertex2] && edges[vertex2, vertex1];
            }
        }
        public object getVertexData(int vertexNumber)
        {
            if (vertexNumber < 0 || vertexNumber >= vertices.Length) // Invalid indexes
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return vertices[vertexNumber];
            }
        }
        public int getNumVertices()
        {
            return vertices.Length;
        }
        public bool isConnected()
        {
            if (getNumVertices() == 0)
            {
                return false;
            }
            foreach (int x in DFSbyREF(0))
            {
                if (x == 0) return false;
            }
            return true;
        }
        public bool hasCycle()
        {
            for (int i = 0; i < getNumVertices(); i++)
            {
                if (DFSbyVAL(i)) return true;
            }
            return false;
        }
        public bool isTree()
        {
            return isConnected() && !hasCycle();
        }
        public override string ToString()
        {
            string add = "";
            for (int i = 0; i < getNumVertices(); i++)
            {
                add += "Value at vertex " + i + ": ";
                add += vertices[i].ToString();
                add += System.Environment.NewLine;

            }

            string edges = "";
            for (int i = 0; i < getNumVertices(); i++)
            {
                for (int j = 0; j < getNumVertices() - 1; j++)
                {
                    if (hasEdge(i, j))
                    {
                        edges += "Vertex " + i + " has edge with " + j;
                    }
                }
            }

            return "Number of vertecies: " + getNumVertices() + System.Environment.NewLine + add + System.Environment.NewLine + edges;
        }

        // Depth-First Search Implementations

        // isConnected()
        private int[] DFSbyREF(int startingIndex)
        {
            int[] orderArray = new int[getNumVertices()];
            int order = 1;
            DFS_Recursive_REF(startingIndex, orderArray, ref order);
            return orderArray;
        }
        private void DFS_Recursive_REF(int currentVertex, int[] orderArray, ref int order)
        {
            orderArray[currentVertex] = order;
            order++;
            for (int vertex_i = 0; vertex_i < getNumVertices(); vertex_i++)
            {
                if (edges[currentVertex, vertex_i] && orderArray[vertex_i] == 0)
                {
                    DFS_Recursive_REF(vertex_i, orderArray, ref order);
                }
            }
        }

        // hasCycle()
        private bool DFSbyVAL(int startingIndex)
        {
            int[] arrayOrder = new int[getNumVertices()];
            int order = 1;
            return DFS_Recursive_VAL(startingIndex, arrayOrder, order);
        }
        private bool DFS_Recursive_VAL(int currentVertex, int[] arrayOrder, int order)
        {
            arrayOrder[currentVertex] = order;
            order++;
            for (int vertex_i = 0; vertex_i < vertices.Length; vertex_i++)
            {
                if (hasEdge(currentVertex, vertex_i)) // Has a neighbor
                {
                    if (arrayOrder[vertex_i] == 0)  // Is univsited
                    {
                        if (DFS_Recursive_VAL(vertex_i, arrayOrder, order)) return true;
                    }
                    else if (arrayOrder[vertex_i] != arrayOrder[currentVertex] - 1)  // Did not come from same edge
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DFSbyVAL2(int startingIndex, int [] v)
        {
            int order = 1;
            return DFS_Recursive_VAL(startingIndex, v, order);
        }

        public int maxDistance(int startIndex)
        {
            int[] distanceArray = new int[getNumVertices()];
            int distance = 0;
            distanceDFS(startIndex, startIndex, distanceArray, distance);
            int curMax = 0;
            for(int i = 0; i < getNumVertices(); i++)
            {
                if (distanceArray[i] > curMax) curMax = distanceArray[i];
            }
            return curMax;

        }

        private void distanceDFS(int startIndex, int currentIndex, int[] distanceArray, int distance)
        {
            distanceArray[currentIndex] = distance;
            distance++;
            for(int i = 0; i < getNumVertices(); i++)
            {
                if(hasEdge(currentIndex, i) && distanceArray[i] == 0 && vertices[i] != vertices[startIndex])
                {
                    distanceDFS(startIndex, i, distanceArray, distance);
                }
            }
        }

        public int numComponents()
        {
            int numCom = 0;
            int[] v = new int[getNumVertices()];

            for(int i = 0; i < getNumVertices(); i++)
            {
                if (v[i] == 0)
                {
                    DFSbyVAL2(i, v);
                    numCom++;
                }
            }
            return numCom;
        }
    }
}
