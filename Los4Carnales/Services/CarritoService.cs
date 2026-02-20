using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class CarritoService
{
    public List<PedidoDetalle> Carrito { get; private set; } = new List<PedidoDetalle>();
    public bool EsDelivery { get; set; } = false;

    
    public event Action? OnChange;

    public void AgregarItem(PedidoDetalle item)
    {
        var existente = Carrito.FirstOrDefault(x => x.ProductoId == item.ProductoId);
        if (existente != null)
        {
           
            existente.Cantidad += item.Cantidad;
            existente.Importe = existente.Cantidad * item.PrecioUnitario;
        }
        else
        {
            Carrito.Add(item);
        }
        NotifyStateChanged();
    }

    public void RemoverItem(PedidoDetalle item)
    {
        Carrito.Remove(item);
        NotifyStateChanged();
    }

    public void LimpiarCarrito()
    {
        Carrito.Clear();
        EsDelivery = false;
        NotifyStateChanged();
    }

    public double ObtenerTotal()
    {
        return Carrito.Sum(x => x.Importe);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
