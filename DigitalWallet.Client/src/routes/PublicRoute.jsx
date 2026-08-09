import { Navigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../context/AuthContext";


function PublicRoute({children})
{

    const {user,loading}=useContext(AuthContext);


    if(loading)
    {
        return <h1>Loading...</h1>;
    }


    if(user)
    {
        if(user.role === "Admin")
        {
            return <Navigate to="/admin" replace />;
        }

        return <Navigate to="/dashboard" replace />;
    }


    return children;
}


export default PublicRoute;