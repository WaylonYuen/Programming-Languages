using System;
using System.Threading;

namespace TimothyLiu_22_EventHandler {

    class Program {
        static void Main(string[] args) {

            Customer customer = new Customer();
            Waiter waiter = new Waiter();

            customer.Order += new EventHandler(waiter.Action);    //服務員訂閱客戶菜單，當客戶下單則響應事件
            customer.Action();
            customer.PayTheBill();
        }
    }

    //事件資料
    public class OrderEventArgs : EventArgs {
        public string DishName { get; set; }
        public string Size { get; set; }
    }

    //public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);

    //客戶
    public class Customer {

        public event EventHandler Order;    //事件宣告

        public double Bill { get; set; }

        public void PayTheBill() {
            Console.WriteLine($"I will pay ${this.Bill}");
        }

        public void WalkIn() {
            Console.WriteLine("Walk into the restaurant.");
        }

        public void SitDown() {
            Console.WriteLine("Sit down.");
        }

        public void Think() {
            for (int i = 0; i < 3; i++) {
                Console.WriteLine("Let me think...");
                Thread.Sleep(1000);
            }

            this.OnOrder("Kongpao Chicken", "large");
        }

        protected void OnOrder(string dishName, string size) {
            if (this.Order != null) {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = dishName;
                e.Size = size;
                this.Order.Invoke(this, e);
            }
        }

        public void Action() {
            Console.Read();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }

    }

    //服務員
    public class Waiter {

        public void Action(object sender, EventArgs e) {

            Customer customer = sender as Customer;
            OrderEventArgs orderInfo = e as OrderEventArgs;

            Console.WriteLine($"I will serve you the dish - {orderInfo.DishName}.");

            double price = 10;

            switch (orderInfo.Size) {

                case "small":
                    price *= 0.5;
                    break;

                case "large":
                    price += 1.5;
                    break;

                default:
                    break;
            }

            customer.Bill += price;
        }
    }

}
