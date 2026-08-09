function PasswordStrength({ password })
{


    function calculateStrength(password)
    {

        let strength = 0;


        if(password.length >= 8)
            strength++;


        if(/[A-Z]/.test(password))
            strength++;


        if(/[a-z]/.test(password))
            strength++;


        if(/\d/.test(password))
            strength++;


        if(/[^A-Za-z0-9]/.test(password))
            strength++;


        return strength;

    }



    const strength =
        calculateStrength(password);



    const labels =
    [
        "",
        "Very Weak",
        "Weak",
        "Medium",
        "Strong",
        "Excellent"
    ];



    return (

        <div className="strength-wrapper">


            <div className="strength-bar">


                <div

                className={`strength-fill strength-${strength}`}

                style={{
                    width:`${strength * 20}%`
                }}

                ></div>


            </div>



            <span
            className={`strength-text strength-${strength}`}
            >

                {
                    labels[strength]
                }

            </span>



        </div>

    );

}


export default PasswordStrength;