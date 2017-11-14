namespace _Compi2_Proyecto2_201314863
{
    partial class Reportes
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
            this.txtNombre = new System.Windows.Forms.Label();
            this.tablaReporte = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.tablaReporte)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNombre
            // 
            this.txtNombre.AutoSize = true;
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(12, 9);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(390, 31);
            this.txtNombre.TabIndex = 0;
            this.txtNombre.Text = "Aqui va el nombre del reporte...";
            this.txtNombre.Click += new System.EventHandler(this.txtNombre_Click);
            // 
            // tablaReporte
            // 
            this.tablaReporte.AllowUserToAddRows = false;
            this.tablaReporte.AllowUserToDeleteRows = false;
            this.tablaReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaReporte.Location = new System.Drawing.Point(12, 60);
            this.tablaReporte.Name = "tablaReporte";
            this.tablaReporte.ReadOnly = true;
            this.tablaReporte.Size = new System.Drawing.Size(823, 413);
            this.tablaReporte.TabIndex = 1;
            // 
            // Reportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 485);
            this.Controls.Add(this.tablaReporte);
            this.Controls.Add(this.txtNombre);
            this.Name = "Reportes";
            this.Text = "Reportes";
            ((System.ComponentModel.ISupportInitialize)(this.tablaReporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtNombre;
        private System.Windows.Forms.DataGridView tablaReporte;
    }
}