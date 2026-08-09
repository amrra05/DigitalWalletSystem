import { Navigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../context/AuthContext";


function AdminRoute({ children })
{

    const { user, loading } =
        useContext(AuthContext);



    if(loading || !user)
    {
        return <h1>Loading...</h1>;
    }

    if(user?.role !== "Admin")
    {
        return <Navigate to="/dashboard" />;
    }



    return children;

}


export default AdminRoute;