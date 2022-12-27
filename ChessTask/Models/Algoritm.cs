using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChessTask
{
    public class Algoritm
    {
        public List<List<Node>> Matrix; //матрица - список из 8 списков, внешний - строки
        public List<List<int>> Path;
        public Algoritm(int alg, int n0, int m0)
        {
            Matrix = new List<List<Node>>();
            for (int i=0; i<8; i++)
            {
                var str = new List<Node>();
                for (int j=0; j<8; j++)
                {
                    str.Add(new Node(i,j));
                }
                Matrix.Add(str);
            }
            
            CreatePath(alg, n0, m0);
            
        }
        public void CreatePath(int alg, int n_0, int m_0)
        {
            Path = new List<List<int>>();

            Path.Add(new List<int> { n_0, m_0 });

            switch (alg)
            {
                case 0:
                        FindNextNode(n_0, m_0, 1); break;
                case 1: 
                        CreateCardPath(n_0, m_0); break;

            }

            //if (Path.Count() == 64 && CheckEdge(Path[Path.Count - 1][0], Path[Path.Count - 1][1], n_0, m_0))
            //{
            //}
        }
        
        public bool FindNextNode(int n, int m, int len)
        {
            int min_cnt = 8;
            int min_ind_x = -1;
            int min_ind_y = -1;
            int min_i = -1;

            List<int> dopM = new List<int>();

            foreach (var t in Matrix[n][m].possibleMoves)
            {
                if (CheckExistInPath(t[0], t[1]))
                {
                    dopM.Add(9);
                }
                else
                {
                    var moves = Matrix[t[0]][t[1]].possibleMoves;
                    int count_moves = moves.Count();
                    for (int i = 0; i < moves.Count(); i++)
                    {
                        if (CheckExistInPath(moves[i][0], moves[i][1]))
                            count_moves--;
                    }
                    if (count_moves == 0 && len != 63)
                        count_moves = 9;
                    dopM.Add(count_moves);
                }
            }
            for (int i = 0; i < dopM.Count(); i++)
            {
                min_cnt = 8;
                min_ind_x = -1;
                min_ind_y = -1;
                min_i = -1;

                for (int j=0; j< dopM.Count() ; j++) 
                {
                    if (dopM[j]< min_cnt)
                    {
                        min_cnt = dopM[j];
                        min_ind_x = Matrix[n][m].possibleMoves[j][0];
                        min_ind_y = Matrix[n][m].possibleMoves[j][1];
                        min_i = j;
                    }
                }
                if ((min_ind_x >= 0 && min_ind_y >= 0) ) 
                {
                    Path.Add(new List<int> { min_ind_x, min_ind_y });
                    if (Path.Count() == 64)
                    {
                        //if (CheckEdge(Path[Path.Count - 1][0], Path[Path.Count - 1][1], n_0, m_0))
                            return true;
                        //else
                        //    return false;
                    }
                    if (FindNextNode(min_ind_x, min_ind_y, len + 1))
                        break;
                    else
                    {
                        Path.Remove(Path[Path.Count() - 1]);
                        dopM[min_i] = 9;
                        continue;
                    }
                }
                else
                    return false;
            }
            return true;

        }
        //public void FillErrorPath()
        //{
        //    var temp = new List<List<int>>();
        //    foreach (var node in Path)
        //    {
        //        temp.Add(new List<int> { node[0], node[1] });
        //    }

        //    //foreach (var e in errorPath)
        //    //{
        //    //    if (e.Count() < temp.Count()) // ошибочные пути меньшей длины нас только и интересуют, их удаляем, они дублируют более длинные
        //    //    {
        //    //        int i;
        //    //        for (i = 0; i < e.Count(); i++)
        //    //        {
        //    //            if (!(e[i][0] == temp[i][0] && e[i][1] == temp[i][1]))
        //    //                break;
        //    //        }
        //    //        if (i == e.Count())
        //    //        {
        //    //            errorPath.Remove(e);
        //    //        }
        //    //    }
        //    //}
        //    errorPath.Add(temp);
        //}
        public bool CheckExistInPath (int n, int m)
        {
            foreach (var node in Path)
            {
                if ((node[0] == n) && (node[1] == m))
                    return true;
            }
            return false;
        }
        //public bool CheckBlackList(int n, int m, int pos) //нужен ли pos?
        //{
        //    foreach (var e in errorPath)
        //    {
        //        if (e.Count() > Path.Count()) // ошибочные пути меньшей длины нас не интересуют
        //        {
        //            int i;
        //            for(i=0; i< Path.Count(); i++)
        //            {
        //                if (!(e[i][0] == Path[i][0] && e[i][1] == Path[i][1]))
        //                    break;
        //            }
        //            if (i == Path.Count())
        //                if (e[Path.Count()][0] == n && e[Path.Count()][1] == m) // если кандидат на следующий элемент траектории тоже совпадает с соответствующим элементом ошибочной траектории
        //                    return false;
        //        }
        //    }
        //    return true;
        //}
        
        public bool CheckEdge (int n_prev, int m_prev, int n_cur, int m_cur) //проверим, что из данной точки с координатами (n_prev, m_prev) существует ребро в точку (n_cur, m_cur)
        {
            var node = Matrix[n_prev][m_prev];
            foreach (var c in node.possibleMoves)
            {
                if ((c[0] == n_cur) && (c[1] == m_cur)) 
                    return true;
            }
            return false;
        }
        public void CreateCardPath(int x0, int y0)
        {
            List<List<List<int>>> Matrix = new List<List<List<int>>>();
            var str = new List<List<int>>(); // 0-вая строка
            str.Add(new List<int> { 1, 2 }); str.Add(new List<int> { 1, 3});
            str.Add(new List<int> { 1, 0 }); str.Add(new List<int> { 2, 2 });
            str.Add(new List<int> { 2, 3 }); str.Add(new List<int> { 2, 4 });
            str.Add(new List<int> { 1, 4 }); str.Add(new List<int> { 2, 6 });// конец 0 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 3, 1 }); str.Add(new List<int> { 0, 3 });// 1-ая строка
            str.Add(new List<int> { 0, 4 }); str.Add(new List<int> { 0, 5 });
            str.Add(new List<int> { 0, 2 }); str.Add(new List<int> { 0, 7 });
            str.Add(new List<int> { 3, 7 }); str.Add(new List<int> { 2, 5 }); // конец 1 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 0, 1 }); str.Add(new List<int> { 0, 0 });// 2-ая строка
            str.Add(new List<int> { 3, 4 }); str.Add(new List<int> { 3, 5 });
            str.Add(new List<int> { 1, 6 }); str.Add(new List<int> { 0, 6 });
            str.Add(new List<int> { 4, 7 }); str.Add(new List<int> { 1, 5 }); // конец 2 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 1, 1 }); str.Add(new List<int> { 5, 0 });// 3-ая строка
            str.Add(new List<int> { 5, 3 }); str.Add(new List<int> { 5, 2 });
            str.Add(new List<int> { 4, 2 }); str.Add(new List<int> { 4, 3 });
            str.Add(new List<int> { 1, 7 }); str.Add(new List<int> { 5, 6 }); // конец 3 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 2, 1 }); str.Add(new List<int> { 2, 0 });// 4-ая строка
            str.Add(new List<int> { 5, 4 }); str.Add(new List<int> { 5, 5 });
            str.Add(new List<int> { 3, 2 }); str.Add(new List<int> { 5, 7 });
            str.Add(new List<int> { 2, 7 }); str.Add(new List<int> { 6, 6 }); // конец 4 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 7, 1 }); str.Add(new List<int> { 3, 0 });// 5-ая строка
            str.Add(new List<int> { 4, 4 }); str.Add(new List<int> { 4, 5 });
            str.Add(new List<int> { 3, 3 }); str.Add(new List<int> { 3, 6 });
            str.Add(new List<int> { 7, 7 }); str.Add(new List<int> { 7, 6 }); // конец 5 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 4, 1 }); str.Add(new List<int> { 4, 0 });// 6-ая строка
            str.Add(new List<int> { 7, 0 }); str.Add(new List<int> { 7, 5 });
            str.Add(new List<int> { 7, 2 }); str.Add(new List<int> { 7, 3 });
            str.Add(new List<int> { 7, 4 }); str.Add(new List<int> { 4, 6 }); // конец 6 строки
            Matrix.Add(str);

            str = new List<List<int>>();
            str.Add(new List<int> { 5, 1 }); str.Add(new List<int> { 6, 3 });// 7-ая строка
            str.Add(new List<int> { 6, 0 }); str.Add(new List<int> { 6, 1 });
            str.Add(new List<int> { 6, 2 }); str.Add(new List<int> { 6, 7 });
            str.Add(new List<int> { 6, 4 }); str.Add(new List<int> { 6, 5 }); // конец 7 строки
            Matrix.Add(str);

            int xn = x0;
            int yn = y0;
            while (Path.Count() < 64)
            {
                AddNewNode(Matrix, xn, yn);
                xn = Path[Path.Count() - 1][0];
                yn = Path[Path.Count() - 1][1];
            }

        }
        public void AddNewNode(List<List<List<int>>> Matrix, int x0, int y0)
        {
            Path.Add(new List<int> { Matrix[x0][y0][0], Matrix[x0][y0][1] });
        }
    }

    public class Node
    {
        public List<List<int>> possibleMoves;
        public Node(int n, int m)
        {
            possibleMoves = new List<List<int>>();
            if ((n+1 >= 0) && (n+1 < 8) && (m+2 >= 0) && (m+2 < 8))
                possibleMoves.Add(new List<int>() { n+1, m+2 });
            if ((n+1 >= 0) && (n+1 < 8) && (m-2 >= 0) && (m-2 < 8))
                possibleMoves.Add(new List<int>() { n+1, m-2 });
            if ((n+2 >= 0) && (n+2 < 8) && (m+1 >= 0) && (m+1 < 8))
                possibleMoves.Add(new List<int>() { n+2, m+1 });
            if ((n+2 >= 0) && (n+2 < 8) && (m-1 >= 0) && (m-1 < 8))
                possibleMoves.Add(new List<int>() { n+2, m-1 });
            if ((n-1 >= 0) && (n-1 < 8) && (m+2 >= 0) && (m+2 < 8))
                possibleMoves.Add(new List<int>() { n-1, m+2 });
            if ((n-1 >= 0) && (n-1 < 8) && (m-2 >= 0) && (m-2 < 8))
                possibleMoves.Add(new List<int>() { n-1, m-2 });
            if ((n-2 >= 0) && (n-2 < 8) && (m+1 >= 0) && (m+1 < 8))
                possibleMoves.Add(new List<int>() { n-2, m+1 });
            if ((n-2 >= 0) && (n-2 < 8) && (m-1 >= 0) && (m-1 < 8))
                possibleMoves.Add(new List<int>() { n-2, m-1 });

        }
    }
}
