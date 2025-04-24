using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AI_2_Perceptron
{
    public class Perceptron
    {
        private double[] weights;
        private double learningRate;
        private string FirstClassName;
        private string SecondClassName;
        public ImageService? imageService = null;

        public Perceptron(ImageService imageService, int inputSize, double learningRate = 0.1)
        {
            this.learningRate = learningRate;
            this.imageService = imageService;
            weights = new double[inputSize + 1]; // +1 для зміщення (bias)
            Random rand = new Random();
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = rand.NextDouble() * 2 - 1; // Ініціалізація ваг випадковими числами [-1,1]
            }
        }

        public void SetFirstClassName(string name)
        {
            FirstClassName = name;
        }

        public void SetSecondClassName(string name)
        {
            SecondClassName = name;
        }

        public void SetLearningRate(double rate)
        {
            learningRate = rate;
        }

        public string getClassName(int value)
        {
            return 0 == value ? FirstClassName : SecondClassName;
        }

        private int Activate(double sum)
        {
            imageService.outToLog($"sum is {sum}");

            return sum >= 0 ? 1 : 0; // Функція активації
        }

        public int Predict(double[] inputs)
        {
            double sum = weights[0]; // Починаємо з bias
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i + 1];
            }
            return Activate(sum);
        }

        public void Train(double[][] trainingData, int[] labels, int epochs)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                int errors = 0;
                for (int i = 0; i < trainingData.Length; i++)
                {
                    int prediction = Predict(trainingData[i]);

                    int error = labels[i] - prediction;
                    if (error != 0)
                    {
                        errors++;
                        weights[0] += learningRate * error; // Оновлення bias
                        for (int j = 0; j < trainingData[i].Length; j++)
                        {
                            weights[j + 1] += learningRate * error * trainingData[i][j];
                        }
                    }
                }
                Console.WriteLine($"Epoch {epoch + 1}: Errors = {errors}");
                if (errors == 0) break;
            }
        }

        public void TrainOne(double[] trainingData, int epochs)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                int errors = 0;
                int prediction = Predict(trainingData);
                string className = getClassName(prediction);

                string weightsJoin = String.Join("; ", Array.ConvertAll(weights, x => Math.Round(x, 3)));
                string weightsString = $"weights: ({weightsJoin})";
                imageService.outToLog(weightsString);

                InputDialog inputDialog = new InputDialog($"Is this '{className}'?");
                inputDialog.ShowDialog();

                int expected = inputDialog.FormResponse ? prediction : prediction == 1 ? 0 : 1;
                int error = expected - prediction;

                if (!inputDialog.FormResponse)
                {
                    imageService.outToLog("run correction");
                    errors++;
                    weights[0] += learningRate * error; // Оновлення bias
                    for (int j = 0; j < trainingData.Length; j++)
                    {
                        weights[j + 1] += learningRate * error * trainingData[j];
                    }

                    weightsJoin = String.Join("; ", Array.ConvertAll(weights, x => Math.Round(x, 3)));
                    weightsString = $"weights after correction: ({weightsJoin})";
                    imageService.outToLog(weightsString);
                }

                Console.WriteLine($"Epoch {epoch + 1}: Errors = {errors}");
            }
        }

        public void DisplayWeights()
        {
            Console.WriteLine("Current weights:");
            for (int i = 0; i < weights.Length; i++)
            {
                Console.WriteLine($"w[{i}] = {weights[i]:F4}");
            }
        }
    }
}
