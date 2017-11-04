namespace _Compi2_Proyecto2_201314863
{
    partial class EditorClase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.txtClase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEliminarAtributo = new System.Windows.Forms.Button();
            this.listAtributos = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnEliminarProcedimiento = new System.Windows.Forms.Button();
            this.listProcedimientos = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAgregarProcedimiento = new System.Windows.Forms.Button();
            this.btnAgregarAtributo = new System.Windows.Forms.Button();
            this.tipo = new System.Windows.Forms.ComboBox();
            this.visibilidad = new System.Windows.Forms.ComboBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGuardar);
            this.groupBox1.Controls.Add(this.txtClase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clase";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(274, 15);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(296, 23);
            this.btnGuardar.TabIndex = 2;
            this.btnGuardar.Text = "Guardar clase";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // txtClase
            // 
            this.txtClase.Location = new System.Drawing.Point(60, 17);
            this.txtClase.Name = "txtClase";
            this.txtClase.Size = new System.Drawing.Size(207, 20);
            this.txtClase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnEliminarAtributo);
            this.groupBox2.Controls.Add(this.listAtributos);
            this.groupBox2.Location = new System.Drawing.Point(13, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 127);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Atributos";
            // 
            // btnEliminarAtributo
            // 
            this.btnEliminarAtributo.Location = new System.Drawing.Point(190, 20);
            this.btnEliminarAtributo.Name = "btnEliminarAtributo";
            this.btnEliminarAtributo.Size = new System.Drawing.Size(75, 95);
            this.btnEliminarAtributo.TabIndex = 1;
            this.btnEliminarAtributo.Text = "Eliminar";
            this.btnEliminarAtributo.UseVisualStyleBackColor = true;
            this.btnEliminarAtributo.Click += new System.EventHandler(this.btnEliminarAtributo_Click);
            // 
            // listAtributos
            // 
            this.listAtributos.FormattingEnabled = true;
            this.listAtributos.Location = new System.Drawing.Point(7, 20);
            this.listAtributos.Name = "listAtributos";
            this.listAtributos.Size = new System.Drawing.Size(176, 95);
            this.listAtributos.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnEliminarProcedimiento);
            this.groupBox3.Controls.Add(this.listProcedimientos);
            this.groupBox3.Location = new System.Drawing.Point(287, 87);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(302, 127);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Procedimientos";
            // 
            // btnEliminarProcedimiento
            // 
            this.btnEliminarProcedimiento.Location = new System.Drawing.Point(223, 20);
            this.btnEliminarProcedimiento.Name = "btnEliminarProcedimiento";
            this.btnEliminarProcedimiento.Size = new System.Drawing.Size(75, 95);
            this.btnEliminarProcedimiento.TabIndex = 1;
            this.btnEliminarProcedimiento.Text = "Eliminar";
            this.btnEliminarProcedimiento.UseVisualStyleBackColor = true;
            this.btnEliminarProcedimiento.Click += new System.EventHandler(this.btnEliminarProcedimiento_Click);
            // 
            // listProcedimientos
            // 
            this.listProcedimientos.FormattingEnabled = true;
            this.listProcedimientos.Location = new System.Drawing.Point(7, 20);
            this.listProcedimientos.Name = "listProcedimientos";
            this.listProcedimientos.Size = new System.Drawing.Size(209, 95);
            this.listProcedimientos.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnAgregarProcedimiento);
            this.groupBox4.Controls.Add(this.btnAgregarAtributo);
            this.groupBox4.Controls.Add(this.tipo);
            this.groupBox4.Controls.Add(this.visibilidad);
            this.groupBox4.Controls.Add(this.txtNombre);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(13, 220);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(576, 112);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Agregar atributo";
            // 
            // btnAgregarProcedimiento
            // 
            this.btnAgregarProcedimiento.Location = new System.Drawing.Point(274, 59);
            this.btnAgregarProcedimiento.Name = "btnAgregarProcedimiento";
            this.btnAgregarProcedimiento.Size = new System.Drawing.Size(296, 32);
            this.btnAgregarProcedimiento.TabIndex = 7;
            this.btnAgregarProcedimiento.Text = "Agregar procedimiento";
            this.btnAgregarProcedimiento.UseVisualStyleBackColor = true;
            this.btnAgregarProcedimiento.Click += new System.EventHandler(this.btnAgregarProcedimiento_Click);
            // 
            // btnAgregarAtributo
            // 
            this.btnAgregarAtributo.Location = new System.Drawing.Point(274, 20);
            this.btnAgregarAtributo.Name = "btnAgregarAtributo";
            this.btnAgregarAtributo.Size = new System.Drawing.Size(296, 32);
            this.btnAgregarAtributo.TabIndex = 6;
            this.btnAgregarAtributo.Text = "Agregar atributo";
            this.btnAgregarAtributo.UseVisualStyleBackColor = true;
            this.btnAgregarAtributo.Click += new System.EventHandler(this.btnAgregarAtributo_Click);
            // 
            // tipo
            // 
            this.tipo.FormattingEnabled = true;
            this.tipo.Items.AddRange(new object[] {
            "Void",
            "Entero",
            "Decimal",
            "Caracter",
            "Booleano",
            "Cadena"});
            this.tipo.Location = new System.Drawing.Point(78, 75);
            this.tipo.Name = "tipo";
            this.tipo.Size = new System.Drawing.Size(174, 21);
            this.tipo.TabIndex = 5;
            // 
            // visibilidad
            // 
            this.visibilidad.FormattingEnabled = true;
            this.visibilidad.Items.AddRange(new object[] {
            "Publico",
            "Privado",
            "Protegido"});
            this.visibilidad.Location = new System.Drawing.Point(78, 47);
            this.visibilidad.Name = "visibilidad";
            this.visibilidad.Size = new System.Drawing.Size(174, 21);
            this.visibilidad.TabIndex = 4;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(78, 20);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(174, 20);
            this.txtNombre.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Tipo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Visibilidad:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nombre: ";
            // 
            // EditorClase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 337);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditorClase";
            this.Text = "EditorClase";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox txtClase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEliminarAtributo;
        private System.Windows.Forms.ListBox listAtributos;
        private System.Windows.Forms.Button btnEliminarProcedimiento;
        private System.Windows.Forms.ListBox listProcedimientos;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAgregarProcedimiento;
        private System.Windows.Forms.Button btnAgregarAtributo;
        private System.Windows.Forms.ComboBox tipo;
        private System.Windows.Forms.ComboBox visibilidad;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}