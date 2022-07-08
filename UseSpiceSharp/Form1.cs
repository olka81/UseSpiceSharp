using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpiceSharp;
using SpiceSharp.Components;
//using SpiceSharp.Entities;
using SpiceSharp.Components.BSIM3Behaviors;
using SpiceSharp.Simulations;
//using SpiceSharp.ParameterSets;


namespace UseSpiceSharp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private BSIM3Model CreateModel(string name)
		{
			var m = new BSIM3Model(name);
			//m.ParameterSets.Get<ModelBaseParameters>().CheckPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "bsim3check.log");
			//ApplyParameters(m, parameters);
			return m;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Build the circuit
			var ckt = new Circuit(
			//				new VoltageSource("V1", "in", "0", new Pulse(0.0, 5.0, 0.01, 1e-3, 1e-3, 0.02, 0.04)),
			//				new Resistor("R1", "in", "out", 10.0e3),
			//				new Capacitor("C1", "out", "0", 1e-6)
			);

			var V2 = new VoltageSource("V2", "2", "0", new Sine(0.0, 1.0, 1.0e6, 0.0, 0.0, 0.0));
			var R1 = new Resistor("R1", "5", "0", 100.0);
			var R2 = new Resistor("R2", "6", "5", 100.0);
			var L1 = new Inductor("L1", "3", "2", 1.0e-9);

			var mos_model = new Mosfet1Model("NMOS_DEFAULT_MODEL");
			var bsim3_model = new SpiceSharp.Components.BSIM3Model("BSIM3_DEFAULT_MODEL");
			//var M1 = new Mosfet1("M1", "3", "5", "0", "0", "NMOS_DEFAULT_MODEL");
			var b1 = new BSIM3("B1", "3", "5", "0", "0");
			b1.Model = "BSIM3_DEFAULT_MODEL";

			ckt.Add(V2);
			ckt.Add(R1);
			ckt.Add(R2);
			ckt.Add(L1);
				//ckt.Add(mos_model);
			//ckt.Add(bsim3_model);
			//ckt.Add(bsim3_model);
			ckt.Add(b1);
			//ckt.Add(M1);

			textBox1.Text = "";
			// Create the simulation
			var tran = new Transient("Tran 1", 1e-8, 1e-4);

			// Make the exports
			var inputExport = new RealVoltageExport(tran, "3");
			//var outputExport = new RealVoltageExport(tran, "out");

			// Simulate
			tran.ExportSimulationData += (sender1, args) =>
			{

				var input = inputExport.Value;
				//var output = outputExport.Value;
				textBox1.Text += args.Time.ToString();
				textBox1.Text += " ";
				//textBox1.Text += output.ToString();
				//textBox1.Text += " ";
				textBox1.Text += input.ToString();
				textBox1.Text += "\r\n";
			};
			//mos_model.CreateBehaviors(tran);

			tran.Run(ckt);
		}
	}
}
