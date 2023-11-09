import { useState } from "react";
import Loading from "../Loading/Loading";
import { ButtonContainer } from "../../Components/Styles/ButtonContainer.styled";
import { ButtonRowContainer } from "../../Components/Styles/ButtonRow.styled";
import { TextContainer } from "../Styles/TextContainer.styled";
import { Form, FormRow } from "../Styles/Form.styled";
import { InputForm, InputWrapper } from "../Styles/Input.styled";

const UserForm = ({ onSave, user, onCancel }) => {

  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState(user?.email ?? "");
  const [username, setUsername] = useState(user?.username ?? "");
  const [password, setPassword] = useState(user?.password ?? "");

  //submit function
  const onSubmit = (e) => {
    e.preventDefault();

    if (user) {
      return onSave({
        ...user,
        email,
        username,
        password
      });
    }

    return onSave({
        email,
        username,
        password
    });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <Form onSubmit={onSubmit}>
      
      <FormRow className="control">
        <TextContainer>E-mail:</TextContainer>
        <InputWrapper>
          <InputForm
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            name="email"
            id="email"
            placeholder="E-mail"
          />
        </InputWrapper>
      </FormRow>

      <FormRow className="control">
        <TextContainer>Username:</TextContainer>
        <InputWrapper>
          <InputForm
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            name="username"
            id="username"
            placeholder="Username"
            autocomplete="off"
          />
        </InputWrapper>
      </FormRow>

      <FormRow className="control">
        <TextContainer>Password:</TextContainer>
        <InputWrapper>
          <InputForm
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            name="password"
            id="password"
            placeholder="Password"
            autocomplete="off"
            type="password"
          />
        </InputWrapper>
      </FormRow>

      <FormRow className="control">
        <TextContainer>Repeat Password:</TextContainer>
        <InputWrapper>
          <InputForm
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            name="password"
            id="password"
            placeholder="Password"
            autocomplete="off"
            type="password"
          />
        </InputWrapper>
      </FormRow>

      <ButtonRowContainer>
        <ButtonContainer type="submit">
          {user ? "Update User" : "Create User"}
        </ButtonContainer>
        <ButtonContainer type="button" onClick = { onCancel }>
          Cancel
        </ButtonContainer>
      </ButtonRowContainer>
    </Form>
  );
};

export default UserForm;
