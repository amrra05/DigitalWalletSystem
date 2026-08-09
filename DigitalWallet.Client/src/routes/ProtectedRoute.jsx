import { Navigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../context/AuthContext";


function ProtectedRoute({ children })
{

    const { token, loading } =
        useContext(AuthContext);



    if(loading)
    {
        return <h1>Loading...</h1>;
    }



        if(!token)
        {
            return (
                <Navigate
                    to="/login"
                    replace
                />
            );
        }



    return children;

}


export default ProtectedRoute;