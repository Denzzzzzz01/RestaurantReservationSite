import * as Yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import './RegisterPage.scss';

type Props = {}

type RegisterFormsInputs = {
    username: string;  
    email: string;
    password: string;
    confirmPassword: string;
};

const validation = Yup.object().shape({
  username: Yup.string().required("Username is required"),
  email: Yup.string().required("Email is required").email("Invalid email address"),
  password: Yup.string().required("Password is required"),
  confirmPassword: Yup.string()
      .required("Password confirmation is required")
      .oneOf([Yup.ref('password')], "Passwords must match"),
});

const RegisterPage = (props: Props) => {
    const { registerUser } = useAuth();
    const { register, handleSubmit, formState: {errors} } = useForm<RegisterFormsInputs>({resolver: yupResolver(validation)});

    const handleRegister = (form: RegisterFormsInputs) => {
        registerUser(form.username, form.email, form.password, form.confirmPassword);
    };

  return (
    <section className="register-page">
      <div className="register-container">
        <div className="register-header">
          <div className="header-image">
            <img src="/images/enter.png" alt="Sign up" />
          </div>
          <h1>Sign up to your account</h1>
        </div>

        <form className="register-form" onSubmit={handleSubmit(handleRegister)}>
          <div className="form-group">
            <div className="input-icon">
              <i className="fi fi-rr-user"></i>
            </div>
            <input
              type="text"
              id="username"
              placeholder="Username"
              {...register("username")}
            />
            {errors.username && (
              <p className="error-message">{errors.username.message}</p>
            )}
          </div>

          <div className="form-group">
            <div className="input-icon">
              <i className="fi fi-rr-envelope"></i>
            </div>
            <input
              type="text"
              id="email"
              placeholder="Email"
              {...register("email")}
            />
            {errors.email && <p className="error-message">{errors.email.message}</p>}
          </div>

          <div className="form-group">
            <div className="input-icon">
              <i className="fi fi-rr-lock"></i>
            </div>
            <input
              type="password"
              id="password"
              placeholder="Password"
              {...register("password")}
            />
            {errors.password && (
              <p className="error-message">{errors.password.message}</p>
            )}
          </div>

          <div className="form-group">
            <div className="input-icon">
              <i className="fi fi-rr-lock"></i>
            </div>
            <input
              type="password"
              id="confirmPassword"
              placeholder="Confirm password"
              {...register("confirmPassword")}
            />
            {errors.confirmPassword && (
              <p className="error-message">{errors.confirmPassword.message}</p>
            )}
          </div>

          <button type="submit" className="register-button">
            Sign up
          </button>
        </form>

        <div className="login-link">
          <p>
            Already have an account? <Link to="/login">Sign in</Link>
          </p>
        </div>
      </div>
    </section>
  )
}

export default RegisterPage