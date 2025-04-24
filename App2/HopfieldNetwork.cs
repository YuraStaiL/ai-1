using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    class HopfieldNetwork
    {
        private int size;
        private double[,] weights;

        public HopfieldNetwork(int size)
        {
            this.size = size;
            weights = new double[size, size];
        }

        // Навчання: Hebbian learning rule
        public void Train(List<int[]> patterns)
        {
            foreach (var p in patterns)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (i != j)
                            weights[i, j] += p[i] * p[j];
                    }
                }
            }
        }

        public int[] Recognize(int[] input, int maxIterations = 100)
        {
            int[] output = (int[])input.Clone();
            for (int step = 0; step < maxIterations; step++)
            {
                bool changed = false;
                for (int i = 0; i < size; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < size; j++)
                    {
                        sum += weights[i, j] * output[j];
                    }
                    int newState = sum >= 0 ? 1 : -1;
                    if (output[i] != newState)
                    {
                        output[i] = newState;
                        changed = true;
                    }
                }
                if (!changed) break;
            }
            return output;
        }

        public void DisplayWeights(DataGridView gridView)
        {
            gridView.Rows.Clear();
            gridView.Columns.Clear();

            for (int i = 0; i < size; i++)
            {
                gridView.Columns.Add($"col{i}", i.ToString());
            }

            for (int i = 0; i < size; i++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(gridView);
                for (int j = 0; j < size; j++)
                {
                    row.Cells[j].Value = weights[i, j].ToString("F1");
                }
                gridView.Rows.Add(row);
            }
        }
    }
}
