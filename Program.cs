using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.Console;

namespace tpl_presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            #region Setup Dataflow Blocks
            var ordersBuffer = new BufferBlock<int>(new DataflowBlockOptions { BoundedCapacity = 20 });

            var cutDough = new TransformBlock<int, Pizza>(
                orderId => {
                    Task.Delay(371).Wait();
                    return new Pizza(orderId);
                },
                new ExecutionDataflowBlockOptions { BoundedCapacity = 1, MaxDegreeOfParallelism = 1 }
            );

            var addIngredients = new TransformBlock<Pizza, Pizza>(
                p => {
                    Task.Delay(457).Wait();
                    p.Ingredients.Add("Sauce");
                    p.Ingredients.Add("Pepperoni");
                    p.Ingredients.Add("Cheese");
                    return p;
                },
                new ExecutionDataflowBlockOptions { BoundedCapacity = 2, MaxDegreeOfParallelism = 2 }
            );

            var bakePizza = new TransformBlock<Pizza, Pizza>(
                p => {
                    Task.Delay(3972).Wait();
                    p.Baked = true;
                    return p;
                },
                new ExecutionDataflowBlockOptions { BoundedCapacity = 8, MaxDegreeOfParallelism = 8 }
            );

            var batchPizza = new BatchBlock<Pizza>(5, new GroupingDataflowBlockOptions { BoundedCapacity = 5 });

            var deliverPizzas = new ActionBlock<Pizza[]>(
                pb => {
                    Task.Delay(1213 * pb.Length).Wait();
                    WriteLine($"Delivered {pb.Length} pizzas");
                },
                new ExecutionDataflowBlockOptions { BoundedCapacity = 3, MaxDegreeOfParallelism = 3 }
            );
            #endregion

            #region Setup Pipeline
            ordersBuffer.LinkTo(cutDough, linkOptions);
            cutDough.LinkTo(addIngredients, linkOptions);
            addIngredients.LinkTo(bakePizza, linkOptions);
            bakePizza.LinkTo(batchPizza, linkOptions);
            batchPizza.LinkTo(deliverPizzas, linkOptions);
            #endregion

            #region Add Orders
            for (int i = 1; i <= 100; i++)
            {
                await ordersBuffer.SendAsync(i);
            }
            #endregion

            await deliverPizzas.Completion;
        }
    }

    public class Pizza
    {
        public Pizza(int orderId)
        {
            this.OrderId = orderId;
            this.Ingredients = new List<string>();
            this.Baked = false;
        }

        public int OrderId { get; set; }
        public List<string> Ingredients { get; set; }
        public bool Baked { get; set; }
    }
}
