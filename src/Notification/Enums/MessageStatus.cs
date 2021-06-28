

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public enum MessageStatus
    {
        /// <summary>
        /// Sin accion tomada aun
        /// </summary>
        None,

        /// <summary>
        /// Cuando esta en el canal de envio
        /// </summary>
        Pending,

        /// <summary>
        /// Cuando fue exitosamente entregado
        /// </summary>
        Delivered,

        /// <summary>
        /// Cuando ha ocurrido un error
        /// </summary>
        Error
    }
}