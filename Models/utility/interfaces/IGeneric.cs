﻿using websocket_api.Models.usuario;
using websocket_api.Models.utility.structView;

namespace websocket_api.Models.utility.interfaces;

public interface IGeneric<in T, TS>
{
    List<GenericView> Find(string data, string param, bool isFecha = false);

    Task<string?> Crud(T? objecto, Usuario user, char opc);
}