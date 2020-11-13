namespace TACSLib
{
    public enum PacketType : byte
    {
        S_SERVER_VER,
        C_SEND_CERT,
        C_LOGIN,
        S_LOGIN_RESULT,
        C_SEND_MESSAGE,
        S_SEND_MESSAGE,
        C_CHANGE_CHARACTER,
        S_STATUS_CHANGE,
        C_UPDATE,
    }
}
