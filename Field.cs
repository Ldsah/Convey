using System;
using System.Drawing;
using System.Windows.Forms;

namespace Convey
{
    class Field
    {
        private int W; // Ширина поля (в клетках)
        private int H; // Высота поля
        int[,] A, B;   // 1 - закрашено, 0 - незакрашено

        public Field(int w, int h) // Конструктор
        {
            H = h; W = w;
            A = new int[W, H];     // Создает основной 
            B = new int[W, H];     // и вспом. массив
            Clear();
        }

        public void Clear() // Обнуляет основное поле
        {
            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                    A[i, j] = 0;
        }

        public int this[int i, int j] // Доступ к полю
        {
            get
            {
                if (i < 0) i += W;
                if (i >= W) i -= W;
                if (j < 0) j += H;
                if (j >= H) j -= H;
                return A[i, j];
            }
            set
            {
                if (i < 0) i += W;
                if (i >= W) i -= W;
                if (j < 0) j += H;
                if (j >= H) j -= H;
                A[i, j] = value;
            }
        }

        public int around(int I, int J) // Вычисление числа соседей
        {
            int z = -this[I,J];
            for (int i = I - 1; i <= I+1; i++)
                for (int j = J - 1; j <= J+1; j++)
                    z += this[i, j];
            return z;
        }

        public void Step(Panel f, int D)  // Один шаг
        {
            for (int i=0; i<W; i++)
                for (int j = 0; j < H; j++)
                {
                    int z = around(i, j);
                    if (z == 3) B[i, j] = 1;  // Правила игры
                    else if (z == 2) B[i, j] = A[i, j];
                    else B[i, j] = 0;
                }

		// Копирование B -> A
            for (int i = 0; i < W; i++) 
                for (int j = 0; j < H; j++)
                {
                    if (A[i, j] != B[i, j]) // Клетка изменила цвет
                    {
                        Rectangle R = 
                            new Rectangle(i * D, j * D, D, D);
                        f.Invalidate(R);
                    }
                    A[i, j] = B[i, j];
                }
        }

	// Заполнение случайными 0 и 1
        public void Rnd() 
        {
            Random R = new Random();
            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                    A[i, j] = R.Next(2);
        }

	// Отрисовка массива, D - размер квадратика
        public void Paint(Graphics g, int D)
        {  
            Brush b;

            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                {
                    if (A[i, j] != 0) 
                        b = Brushes.Black;
                    else 
                        b = Brushes.White;
                    g.FillRectangle(b, i * D, j * D, D-1, D-1);
                }
         

        }

    }
}
