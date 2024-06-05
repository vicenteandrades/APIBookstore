using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBookstore.Migrations
{
    /// <inheritdoc />
    public partial class now : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Pedidos_PedidoId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_PedidoId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "Pedidos",
                newName: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Pedidos",
                newName: "IdCliente");

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_PedidoId",
                table: "Produtos",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Pedidos_PedidoId",
                table: "Produtos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "PedidoId");
        }
    }
}
