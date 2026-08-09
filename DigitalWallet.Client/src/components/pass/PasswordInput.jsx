import { useState } from "react";


function PasswordInput({
    value,
    setValue,
    placeholder
})
{


    const [show,setShow] = useState(false);



    return (

        <div className="password-container">


            <input

            type={
                show
                ?
                "text"
                :
                "password"
            }

            value={value}

            placeholder={placeholder}

            onChange={(e)=>
                setValue(
                    e.target.value
                )
            }

            />



            <button

            type="button"

            className="show-password-btn"

            onClick={()=>
                setShow(!show)
            }

            >

            {
                show
                ?
                "Hide"
                :
                "Show"
            }

            </button>



        </div>

    );

}


export default PasswordInput;