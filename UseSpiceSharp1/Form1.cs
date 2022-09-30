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
			//Test1();
			Test2();
		}

		private void Test2()
		{
			var ckt = new Circuit(
			);
			//Vd d 0 dc 2.0
			var Vd = new VoltageSource("Vd", "d", "0", 2.0);
			//vin in 0 pulse(0 2 2ns 2ns 2ns 10ns 20ns)
			var vin = new VoltageSource("vin", "in", "0", new Pulse(0, 2, 2e-9, 2e-9, 2e-9, 10e-9, 20e-9));
			//m1 out in 0 0 n1 L=0.35u W=10.0u off
			var bsim3_model = CreateBSIM3Model("BSIM3_DEFAULT_MODEL", "tnom=27.0 elm=3 nch=2.498E+17  tox=9E-09 xj=1.00000E-07 Lint = 9.36e-8 Wint = 1.47e-7 Lintnoi = 1e-9 Vth0 = .6322    K1 = .756  K2 = -3.83e-2  K3 = -2.612  Dvt0 = 2.812  Dvt1 = 0.462  Dvt2 = -9.17e-2  Nlx = 3.52291E-08  W0 = 1.163e-6  K3b = 2.233  Vsat = 86301.58  Ua = 6.47e-9  Ub = 4.23e-18  Uc = -4.706281E-11  Rdsw = 650  U0 = 388.3203 wr = 1  A0 = .3496967 Ags = .1    B0 = 0.546    B1 = 1  Dwg = -6.0E-09 Dwb = -3.56E-09 Prwb = -.213  Keta = -3.605872E-02  A1 = 2.778747E-02  A2 = .9  Voff = -6.735529E-02  NFactor = 1.139926  Cit = 1.622527E-04  Cdsc = -2.147181E-05  Cdscb = 0  Dvt0w = 0 Dvt1w = 0 Dvt2w = 0  Cdscd = 0 Prwg = 0  Eta0 = 1.0281729E-02  Etab = -5.042203E-03  Dsub = .31871233  Pclm = 1.114846  Pdiblc1 = 2.45357E-03  Pdiblc2 = 6.406289E-03  Drout = .31871233  Pscbe1 = 5000000  Pscbe2 = 5E-09 Pdiblcb = -.234  Pvag = 0 delta = 0.01  Wl = 0 Ww = -1.420242E-09 Wwl = 0  Wln = 0 Wwn = .2613948 Ll = 1.300902E-10  Lw = 0 Lwl = 0 Lln = .316394  Lwn = 0  kt1 = -.3  kt2 = -.051  At = 22400  Ute = -1.48  Ua1 = 3.31E-10  Ub1 = 2.61E-19 Uc1 = -3.42e-10  Kt1l = 0  Prt = 764.3"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
			var m1 = CreateBSIM3("m1", "out", "in", "0", "0", 10e-6, 0.35e-6, "BSIM3_DEFAULT_MODEL");
			//m2 out in d d p1 L=0.35u W=10.0u off
			var bsim3_model_p = CreateBSIM3Model("BSIM3_DEFAULT_MODEL_1", "tnom=27.0 elm=3 nch=2.498E+17  tox=9E-09 xj=1.00000E-07 Lint = 9.36e-8 Wint = 1.47e-7 Lintnoi = 1e-9 Vth0 = .6322    K1 = .756  K2 = -3.83e-2  K3 = -2.612  Dvt0 = 2.812  Dvt1 = 0.462  Dvt2 = -9.17e-2  Nlx = 3.52291E-08  W0 = 1.163e-6  K3b = 2.233  Vsat = 86301.58  Ua = 6.47e-9  Ub = 4.23e-18  Uc = -4.706281E-11  Rdsw = 650  U0 = 388.3203 wr = 1  A0 = .3496967 Ags = .1    B0 = 0.546    B1 = 1  Dwg = -6.0E-09 Dwb = -3.56E-09 Prwb = -.213  Keta = -3.605872E-02  A1 = 2.778747E-02  A2 = .9  Voff = -6.735529E-02  NFactor = 1.139926  Cit = 1.622527E-04  Cdsc = -2.147181E-05  Cdscb = 0  Dvt0w = 0 Dvt1w = 0 Dvt2w = 0  Cdscd = 0 Prwg = 0  Eta0 = 1.0281729E-02  Etab = -5.042203E-03  Dsub = .31871233  Pclm = 1.114846  Pdiblc1 = 2.45357E-03  Pdiblc2 = 6.406289E-03  Drout = .31871233  Pscbe1 = 5000000  Pscbe2 = 5E-09 Pdiblcb = -.234  Pvag = 0 delta = 0.01  Wl = 0 Ww = -1.420242E-09 Wwl = 0  Wln = 0 Wwn = .2613948 Ll = 1.300902E-10  Lw = 0 Lwl = 0 Lln = .316394  Lwn = 0  kt1 = -.3  kt2 = -.051  At = 22400  Ute = -1.48  Ua1 = 3.31E-10  Ub1 = 2.61E-19 Uc1 = -3.42e-10  Kt1l = 0  Prt = 764.3"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
			var m2 = CreateBSIM3("m2", "out", "in", "d", "d", 10e-6, 0.35e-6, "BSIM3_DEFAULT_MODEL_1");
			//SetPmos//SetPmos
			bsim3_model_p.SetParameter("pmos", true);

			

			ckt.Add(Vd);
			ckt.Add(vin);
			ckt.Add(m1);
			ckt.Add(m2);
			ckt.Add(bsim3_model);
			ckt.Add(bsim3_model_p);

			var tran = new Transient("Tran 1", 0.01e-9, 20e-9);
			//// Make the exports
			////var inputExport = new RealVoltageExport(tran, "3");

			var outputExport = new RealVoltageExport(tran, "in");
			var outputExport1 = new RealVoltageExport(tran, "out");

			//// Simulate
			tran.ExportSimulationData += (sender1, args) =>
			{

				//var input = inputExport.Value;
				var output = outputExport.Value;
				var output1 = outputExport1.Value;
				textBox1.Text += args.Time.ToString();
				textBox1.Text += " ";
				textBox1.Text += output.ToString();
				textBox1.Text += " ";
				textBox1.Text += output1.ToString();
				//textBox1.Text += " ";
				//textBox1.Text += input.ToString();
				textBox1.Text += "\r\n";
			};
			tran.Run(ckt);

		}

		private void Test1()
		{ 
			// Build the circuit
			var ckt = new Circuit(
			);

			var vgs = new VoltageSource("vgs", "1", "0", 3.5);
			var vdd = new VoltageSource("vdd", "2", "0", 3.5);
			var vss = new VoltageSource("vss", "4", "0", 0);
			var i1 = new CurrentSource("i1", "2", "3", 0.001);

			var bsim3_model = CreateBSIM3Model("BSIM3_DEFAULT_MODEL", "tnom=27.0 elm=3 nch=2.498E+17  tox=9E-09 xj=1.00000E-07 Lint = 9.36e-8 Wint = 1.47e-7 Lintnoi = 1e-9 Vth0 = .6322    K1 = .756  K2 = -3.83e-2  K3 = -2.612  Dvt0 = 2.812  Dvt1 = 0.462  Dvt2 = -9.17e-2  Nlx = 3.52291E-08  W0 = 1.163e-6  K3b = 2.233  Vsat = 86301.58  Ua = 6.47e-9  Ub = 4.23e-18  Uc = -4.706281E-11  Rdsw = 650  U0 = 388.3203 wr = 1  A0 = .3496967 Ags = .1    B0 = 0.546    B1 = 1  Dwg = -6.0E-09 Dwb = -3.56E-09 Prwb = -.213  Keta = -3.605872E-02  A1 = 2.778747E-02  A2 = .9  Voff = -6.735529E-02  NFactor = 1.139926  Cit = 1.622527E-04  Cdsc = -2.147181E-05  Cdscb = 0  Dvt0w = 0 Dvt1w = 0 Dvt2w = 0  Cdscd = 0 Prwg = 0  Eta0 = 1.0281729E-02  Etab = -5.042203E-03  Dsub = .31871233  Pclm = 1.114846  Pdiblc1 = 2.45357E-03  Pdiblc2 = 6.406289E-03  Drout = .31871233  Pscbe1 = 5000000  Pscbe2 = 5E-09 Pdiblcb = -.234  Pvag = 0 delta = 0.01  Wl = 0 Ww = -1.420242E-09 Wwl = 0  Wln = 0 Wwn = .2613948 Ll = 1.300902E-10  Lw = 0 Lwl = 0 Lln = .316394  Lwn = 0  kt1 = -.3  kt2 = -.051  At = 22400  Ute = -1.48  Ua1 = 3.31E-10  Ub1 = 2.61E-19 Uc1 = -3.42e-10  Kt1l = 0  Prt = 764.3"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
																																																			 //var M1 = new Mosfet1("M1", "3", "5", "0", "0", "NMOS_DEFAULT_MODEL");
			var b3 = CreateBSIM3("B3", "3", "1", "4", "0", 10e-6, 0.35e-6,  "BSIM3_DEFAULT_MODEL");

			ckt.Add(vgs);
			ckt.Add(vdd);
			ckt.Add(vss);
			ckt.Add(i1);
			ckt.Add(bsim3_model);

			ckt.Add(b3);

			textBox1.Text = "";
			// Create the simulation
			var tran = new Transient("Tran 1", 1e-7, 1e-6);
			//// Make the exports
			////var inputExport = new RealVoltageExport(tran, "3");

			var outputExport = new RealVoltageExport(tran, "3");

			//// Simulate
			tran.ExportSimulationData += (sender1, args) =>
			{

				//var input = inputExport.Value;
				var output = outputExport.Value;
				textBox1.Text += args.Time.ToString();
				textBox1.Text += " ";
				textBox1.Text += output.ToString();
				//textBox1.Text += " ";
				//textBox1.Text += input.ToString();
				textBox1.Text += "\r\n";
			};
			tran.Run(ckt);

			// Create simulations
			//var dc = new DC("dc", new[]
			//{
			//	new SweepConfiguration("vgs", 1, 3.5, 0.5)
			//});

			//// Create exports
			//var outputExport = new RealVoltageExport(dc, "3");
			//dc.ExportSimulationData += (sender, args) =>
			//{
			//	var v1 = outputExport.Value;
			//	textBox1.Text += args.SweepValue.ToString();
			//	textBox1.Text += " ";
			//	textBox1.Text += v1.ToString();
			//	textBox1.Text += "\r\n";

			//};
			//dc.Run(ckt);
		}
	}
}
