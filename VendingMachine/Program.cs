// See https://aka.ms/new-console-template for more information

using VendingMachine;
using VendingMachine.Interfaces;

static void DisplayVendingResponseToScreen(VendingResponse response)
{
    Console.WriteLine("Message : " + response.Message);
    Console.WriteLine("IsRejectedCoin : " + response.IsRejectedCoin);

    if (response.RejectedCoin != null)
    {
        Console.WriteLine("The Coin Diameter: " + response.RejectedCoin.Diameter);
        Console.WriteLine("The Coin Weight: " + response.RejectedCoin.Weight);
        Console.WriteLine("The Coin Thickness: " + response.RejectedCoin.Thickness);
    }

    if (response.Change != null)
        DisplayItemChangeToScreen(response.Change);

    Console.WriteLine("IsSuccess : " + response.IsSuccess);
}

static void DisplayItemChangeToScreen(IEnumerable<ItemChange> change)
{
    Console.WriteLine("\nChange Returned\n");

    foreach (var item in change)
    {
        switch (item.Type)
        {
            case CoinType.FiveCent:
                Console.WriteLine("Number Of 5 Cents : " + item.Number);
                break;
            case CoinType.TenCent:
                Console.WriteLine("Number Of 10 Cents : " + item.Number);
                break;
            case CoinType.TwentyFiveCent:
                Console.WriteLine("Number Of 25 Cents : " + item.Number);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

var vm = new VendingMachineProcessor(new CoinService(), new ProductService(new ProductRepository(), new ProductInventoryRepository()));

//case 1 - invalid coins            
Console.WriteLine("\ncase 1 - invalid coins\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 3.25m, Diameter = 18.0m, Thickness = 1.70000m }));

//case 2 - valid coins,return coins            
Console.WriteLine("\ncase 2 - valid coins,return coins\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 5.00m, Diameter = 20.5m, Thickness = 1.95m }));
DisplayItemChangeToScreen(vm.ReturnCoins());

//case 3 - valid coins, invalid product code           
Console.WriteLine("\ncase 3 - valid coins, invalid product code\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 5.00m, Diameter = 20.5m, Thickness = 1.95m }));
DisplayVendingResponseToScreen(vm.SelectProduct("COOO1"));

//case 4 - valid coins, valid product code, less amount entered            
Console.WriteLine("\ncase 4 - valid coins, valid product code, less amount entered\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 5.00m, Diameter = 20.5m, Thickness = 1.95m }));
DisplayVendingResponseToScreen(vm.SelectProduct("COLA1"));

//case 5 - valid coins, valid product code, more amount entered, make change            
Console.WriteLine("\ncase 5 - valid coins, valid product code, more amount entered, make change\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 5.67m, Diameter = 24.257m, Thickness = 1.956m }));
DisplayVendingResponseToScreen(vm.SelectProduct("COLA1"));

//case 6 - valid coins, valid product code, correct(>=) amount entered, sold out            
Console.WriteLine("\ncase 6 - valid coins, valid product code, correct(>=) amount entered, sold out\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 2.268m, Diameter = 17.91m, Thickness = 1.35m }));
DisplayVendingResponseToScreen(vm.SelectProduct("CHIPS1"));

//case 7 - valid coins, valid product code, correct(>=) amount entered, sold out, return coins            
Console.WriteLine("\ncase 7 - valid coins, valid product code, correct(>=) amount entered, sold out, return coins\n");
DisplayVendingResponseToScreen(vm.AcceptCoin(new InputCoin() { Weight = 2.268m, Diameter = 17.91m, Thickness = 1.35m }));
DisplayVendingResponseToScreen(vm.SelectProduct("CHIPS1"));
DisplayItemChangeToScreen(vm.ReturnCoins());

Console.ReadLine();