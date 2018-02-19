using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Xamarin.Android;
using System;
using Android.Text;

namespace XamCalc
{
    [Activity(Label = "Simple Calculator", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private TextView calculatorText;
        private string[] numbers = new string[2];
        private string @operator;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view);
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.option_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_support:
                    Toast.MakeText(ApplicationContext, "test", ToastLength.Long).Show();
                    return true;
                case Resource.Id.action_info:
                    string myHtmlText = "<p>App made by Patrick Daniel to test Xamarin and AdMob. GitHub: <a href='https://github.com/patrickHD'>patrickHD</a> Website: <a href='http://patrickdaniel.tech'>PatrickDaniel.tech</a></p>";
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("About the Dev");
                    alert.SetMessage(message: Html.FromHtml(myHtmlText));
                    alert.SetButton("OK", (c, ev) =>
                    {

                    });
                    alert.Show();
                    return true;
                case Resource.Id.action_exit:
                    System.Environment.Exit(0);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddDigitOrDecimalPoint(button.Text);
            else if ("÷x+-".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();

        }

        private void Erase()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);
            switch (@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "x":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }

            if(result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        private void AddOperator(string value)
        {
            if(numbers[1] != null)
            {
                Calculate(value);
                return;
            }
            @operator = value;
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;
            if (value == "." && numbers[index].Contains("."))
                return;

            numbers[index] += value;

            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";
    }
}

