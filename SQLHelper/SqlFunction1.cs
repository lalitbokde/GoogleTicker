//------------------------------------------------------------------------------
// <copyright file="CSSqlFunction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Linq;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt64 IP2long(SqlString ipAddress)
    {
        // Put your code here
        return new SqlInt64(BitConverter.ToUInt32(System.Net.IPAddress.Parse(ipAddress.Value).GetAddressBytes().Reverse().ToArray(), 0));
    }
}
