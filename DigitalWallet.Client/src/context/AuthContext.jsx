import {
    createContext,
    useState,
    useEffect
}
from "react";

import { getCurrentUser } from "../services/userService";



const AuthContext = createContext();


export function AuthProvider({ children })
{

    const [user, setUser] = useState(null);


    const [token, setToken] = useState(
        () =>
        localStorage.getItem("accessToken")
    );


    const [loading, setLoading] = useState(true);



        async function loginUser(accessToken)
        {

            localStorage.setItem(
                "accessToken",
                accessToken
            );


            setToken(accessToken);


            try
            {

                const currentUser =
                    await getCurrentUser();


                setUser(currentUser);

            }
            catch(error)
            {

                console.log(error);

            }

        }




    function logout()
    {

        localStorage.removeItem(
            "accessToken"
        );


        setToken(null);

        setUser(null);

    }




    useEffect(() =>
    {

        async function loadUser()
        {

            if(!token)
            {
                setLoading(false);
                return;
            }


            try
            {

                const currentUser =
                    await getCurrentUser();


                setUser(currentUser);


            }
            catch(error)
            {

                console.log(
                    "User loading failed",
                    error
                );


                logout();

            }
            finally
            {

                setLoading(false);

            }

        }



        loadUser();


    }, [token]);






    return (

        <AuthContext.Provider

            value={{

                user,

                setUser,

                token,

                loginUser,

                logout,

                loading

            }}

        >

            {children}

        </AuthContext.Provider>

    );

}


export default AuthContext;