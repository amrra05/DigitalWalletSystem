import { useNavigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth";

import "./DashboardComponents.css";


function Navbar({ user })
{

    const { logout } = useAuth();

    const navigate = useNavigate();



    function handleLogout()
    {

        logout();

        navigate("/login");

    }



    return (

        <header className="dashboard-header">


            <h1>
                Digital Wallet
            </h1>



            <div className="header-user">


                <span>
                    🔔
                </span>



                <span>
                    👤 {user?.name}
                </span>



                <button
                    onClick={handleLogout}
                >

                    Logout

                </button>



            </div>



        </header>

    );

}


export default Navbar;