import "./DashboardComponents.css";


function ProfileCard({ user })
{

    return (

        <div className="card profile-card">


            <h2>
                User Profile
            </h2>


            <p>
                <strong>
                    Name:
                </strong>

                {user?.name}

            </p>


            <p>
                <strong>
                    Email:
                </strong>

                {user?.email}

            </p>


            <p>
                <strong>
                    Role:
                </strong>

                {user?.role}

            </p>


        </div>

    );

}


export default ProfileCard;