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
using System.Text.RegularExpressions;
//using SpiceSharp.Entities;
using SpiceSharp.Components.BSIM3Behaviors;
using SpiceSharp.Simulations;
//using SpiceSharp.ParameterSets;
using SpiceSharp.Circuits;

namespace UseSpiceSharp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private BSIM3 CreateBSIM3(string name, string drain, string gate, string source, string bulk, double w, double l, string model)
		{
			// Create the device
			var e = new BSIM3(name, drain, gate, source, bulk);
			e.SetParameter("w", w);
			e.SetParameter("l", l);
			e.Model = model;
			return e;
		}

		private BSIM1 CreateBSIM1(string name, string drain, string gate, string source, string bulk, double w, double l, string model)
		{
			// Create the device
			var e = new BSIM1(name, drain, gate, source, bulk);
			e.SetParameter("w", w);
			e.SetParameter("l", l);
			e.Model = model;
			return e;
		}

		private BSIM1Model CreateBSIM1Model(string name, string parameters)
		{
			var m = new BSIM1Model(name);
			ApplyParameters(m, parameters);
			return m;
		}

		private BSIM3Model CreateBSIM3Model(string name, string parameters)
		{
			var m = new BSIM3Model(name);
			ApplyParameters(m, parameters);
			return m;
		}

		protected void ApplyParameters(Entity entity, string definition)
		{
			// Get all assignments
			definition = Regex.Replace(definition, @"\s*\=\s*", "=");
			string[] assignments = definition.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var assignment in assignments)
			{
				// Get the name and value
				string[] parts = assignment.Split('=');
				if (parts.Length != 2)
					throw new Exception("Invalid assignment");
				string name = parts[0].ToLower();
				double value = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

				// Set the entity parameter
				entity.SetParameter(name, value);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Build the circuit
			var ckt = new Circuit(
			//				new VoltageSource("V1", "in", "0", new Pulse(0.0, 5.0, 0.01, 1e-3, 1e-3, 0.02, 0.04)),
			//				new Resistor("R1", "in", "out", 10.0e3),
			//				new Capacitor("C1", "out", "0", 1e-6)
			);

			//var V2 = new VoltageSource("V2", "2", "0", new Sine(0.0, 1.0, 1.0e6, 0.0, 0.0, 0.0));
			//var R1 = new Resistor("R1", "5", "0", 100.0);
			//var R2 = new Resistor("R2", "6", "5", 100.0);
			//var L1 = new Inductor("L1", "3", "2", 1.0e-9);

			//var V1 = new VoltageSource("V1", "vdd", "0", 3.3);
			//var V2 = new VoltageSource("V2", "in", "0", new Pulse(0, 3.3, 1e-6, 1e-9, 0.5e-6, 2e-6, 6e-6));
			//         var R1 = new Resistor("R1", "vdd", "out", 10.0e3);
			//         var C1 = new Capacitor("C1", "out", "0", 1e-7);

			var vgs = new VoltageSource("vgs", "1", "0", 3.5);
			var vdd = new VoltageSource("vdd", "2", "0", 3.5);
			var vss = new VoltageSource("vss", "4", "0", 0);
			var i1 = new CurrentSource("i1", "2", "3", 0.001);


			//var mos_model = new Mosfet1Model("NMOS_DEFAULT_MODEL");
			//var bsim1_model = CreateBSIM1Model("BSIM1_DEFAULT_MODEL", "temp=25 muz=600 vdd=5 vfb=-0.3 phi=0.6 k1=0.5 u0=670 x2e=-0.07 mus=1082 n0=0.5 tox=1e-7 mj=0.5 mjsw=0.33 pb=0.8 pbsw=1.0 xpart=1.0"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
			var bsim3_model = CreateBSIM3Model("BSIM3_DEFAULT_MODEL", "tnom=27.0 elm=3 nch=2.498E+17  tox=9E-09 xj=1.00000E-07 Lint = 9.36e-8 Wint = 1.47e-7 Lintnoi = 1e-9 Vth0 = .6322    K1 = .756  K2 = -3.83e-2  K3 = -2.612  Dvt0 = 2.812  Dvt1 = 0.462  Dvt2 = -9.17e-2  Nlx = 3.52291E-08  W0 = 1.163e-6  K3b = 2.233  Vsat = 86301.58  Ua = 6.47e-9  Ub = 4.23e-18  Uc = -4.706281E-11  Rdsw = 650  U0 = 388.3203 wr = 1  A0 = .3496967 Ags = .1    B0 = 0.546    B1 = 1  Dwg = -6.0E-09 Dwb = -3.56E-09 Prwb = -.213  Keta = -3.605872E-02  A1 = 2.778747E-02  A2 = .9  Voff = -6.735529E-02  NFactor = 1.139926  Cit = 1.622527E-04  Cdsc = -2.147181E-05  Cdscb = 0  Dvt0w = 0 Dvt1w = 0 Dvt2w = 0  Cdscd = 0 Prwg = 0  Eta0 = 1.0281729E-02  Etab = -5.042203E-03  Dsub = .31871233  Pclm = 1.114846  Pdiblc1 = 2.45357E-03  Pdiblc2 = 6.406289E-03  Drout = .31871233  Pscbe1 = 5000000  Pscbe2 = 5E-09 Pdiblcb = -.234  Pvag = 0 delta = 0.01  Wl = 0 Ww = -1.420242E-09 Wwl = 0  Wln = 0 Wwn = .2613948 Ll = 1.300902E-10  Lw = 0 Lwl = 0 Lln = .316394  Lwn = 0  kt1 = -.3  kt2 = -.051  At = 22400  Ute = -1.48  Ua1 = 3.31E-10  Ub1 = 2.61E-19 Uc1 = -3.42e-10  Kt1l = 0  Prt = 764.3"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
																																																			 //var M1 = new Mosfet1("M1", "3", "5", "0", "0", "NMOS_DEFAULT_MODEL");
// capmod = 3
			//var M1 = new Mosfet1("M1", "out", "in", "0", "0", "NMOS_DEFAULT_MODEL");
			//var b1 = CreateBSIM1("B1", "out", "in", "0", "0", 100e-6, 100e-6, "BSIM1_DEFAULT_MODEL");
			//var b3 = CreateBSIM3("B3", "out", "in", "0", "0", 100e-6, 100e-6, "BSIM3_DEFAULT_MODEL");
			var b3 = CreateBSIM3("B3", "3", "1", "4", "0", 0.35e-6, 10e-6, "BSIM3_DEFAULT_MODEL");

			//ckt.Add(V2);
			//ckt.Add(R1);
			//ckt.Add(R2);
			//ckt.Add(L1);
			//ckt.Add(mos_model);
			//ckt.Add(V1);
			//ckt.Add(V2);
			//ckt.Add(R1);
			//ckt.Add(C1);
			//ckt.Add(bsim1_model);
			ckt.Add(vgs);
			ckt.Add(vdd);
			ckt.Add(vss);
			ckt.Add(i1);
			ckt.Add(bsim3_model);
			//ckt.Add(b1);
			ckt.Add(b3);
			ckt["B3"].SetParameter("l", 3.5e-7);
			ckt["B3"].SetParameter("w", 1e-6);
			//ckt.Add(M1);

			textBox1.Text = "";
			// Create the simulation
			//var tran = new Transient("Tran 1", 1e-9, 9e-6);
			//// Make the exports
			////var inputExport = new RealVoltageExport(tran, "3");

			//var outputExport = new RealVoltageExport(tran, "out");

			//// Simulate
			//tran.ExportSimulationData += (sender1, args) =>
			//{

			//	//var input = inputExport.Value;
			//	var output = outputExport.Value;
			//	textBox1.Text += args.Time.ToString();
			//	textBox1.Text += " ";
			//	textBox1.Text += output.ToString();
			//	//textBox1.Text += " ";
			//	//textBox1.Text += input.ToString();
			//	textBox1.Text += "\r\n";
			//};
			//tran.Run(ckt);

			//var dc = new DC("dc", new[]
			//{
			//	new SweepConfiguration("vgs", 1, 3.5, 0.5)
			//});
			//var outputExport = new RealVoltageExport(dc, "V(1)");

			//Export<double>[] exports = { new RealPropertyExport(dc, "V(1)", "I(vgs)") };
			//dc.ExportSimulationData += (sender1, args) =>
			//{

			//	//var input = inputExport.Value;
			//	var output = outputExport.Value;
			//	//textBox1.Text += args.Time.ToString();
			//	//textBox1.Text += " ";
			//	textBox1.Text += output.ToString();
			//	//textBox1.Text += " ";
			//	//textBox1.Text += input.ToString();
			//	textBox1.Text += "\r\n";
			//};
			//dc.Run(ckt);


			// Create simulations
			var dc = new DC("dc", new[]
			{
				new SweepConfiguration("vgs", 1, 3.5, 0.5)
			});

			// Create exports
			var outputExport = new RealVoltageExport(dc, "3");
			dc.ExportSimulationData += (sender, args) =>
			{
				var v1 = outputExport.Value;
				textBox1.Text += args.SweepValue.ToString();
				textBox1.Text += " ";
				textBox1.Text += v1.ToString();
				textBox1.Text += "\r\n";

			};
		//	var opExportV1 = new RealPropertyExport(op, "V(1)", "i1");
			//op.ExportSimulationData += (sender, args) =>
			//{
//				var v1 = opExportV1.Value;
	//		};

			// Run DC and op
			//op.Run(ckt);
			dc.Run(ckt);
		}
	}
}
