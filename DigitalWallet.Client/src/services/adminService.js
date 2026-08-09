import apiClient from "../api/apiClient";


export async function getAdminDashboard()
{
    const response =
        await apiClient.get(
            "/admin/dashboard"
        );


    return response.data;
}

export async function getUsers(page = 1, pageSize = 10) {

    const response =
        await apiClient.get(
            `/admin/users?page=${page}&pageSize=${pageSize}`
        );

    return response.data;
}

export async function getAdminUsers(params)
{

    const response =
        await apiClient.get(
            "/admin/users",
            {
                params
            }
        );


    return response.data;

}

export async function freezeUser(id)
{
    const response =
        await apiClient.put(
            `/admin/users/${id}/freeze`
        );

    return response.data;
}



export async function unfreezeUser(id)
{
    const response =
        await apiClient.put(
            `/admin/users/${id}/unfreeze`
        );

    return response.data;
}

export async function resetUserPassword(id, data)
{
    const response =
        await apiClient.put(
            `/admin/users/${id}/reset-password`,
            data
        );

    return response.data;
}

export async function getAdminTransactions(params)
{

    const response =
        await apiClient.get(
            "/admin/transactions",
            {
                params
            }
        );


    return response.data;

}

export async function getAdminAuditLogs({
    page = 1,
    pageSize = 10,
    search = "",
    sortOrder = "desc"
})
{
    const response =
        await apiClient.get(
            "/admin/auditlogs",
            {
                params: {
                    page,
                    pageSize,
                    search,
                    sortOrder
                }
            }
        );


    return response.data;
}