import apiClient from "../api/apiClient";


export async function login(data)
{

    const response =
        await apiClient.post(
            "/auth/login",
            data
        );


    return response.data;

}

export async function changePassword(data)
{

    const response =
        await apiClient.post(
            "/auth/change-password",
            data
        );


    return response.data;

}

export async function register(data)
{

    const response =
        await apiClient.post(
            "/auth/register",
            data
        );


    return response.data;

}

export async function forgotPassword(data)
{

    const response =
        await apiClient.post(
            "/auth/forgot-password",
            data
        );


    return response.data;

}

export async function resetPassword(data)
{

    const response =
        await apiClient.post(
            "/auth/reset-password",
            data
        );


    return response.data;

}