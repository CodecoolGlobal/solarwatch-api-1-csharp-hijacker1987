import { useState } from "react";
import { ButtonContainer } from "../Styles/ButtonContainer.styled";
import { ButtonRowButtonContainer } from "../../Components/Styles/ButtonRow.styled";
import { TextContainer } from "../Styles/TextContainer.styled";
import { FormRow, Form } from "../Styles/Form.styled";
import { InputForm, InputWrapper, SelectForm } from "../Styles/Input.styled";
import axios from 'axios';

const suggestions = ["Kecel", "Kecskemet"]; // Your autocomplete options

const SolarWatch = ({ onGet, onPost, onDelete, country, onCancel }) => {

  const [getCountryName, setGetCountryName] = useState(country?.name ?? "");
  const [deleteCountryName, setDeleteCountryName] = useState(country?.name ?? "");
  const [postCountryName, setPostCountryName] = useState(country?.name ?? "");
  const [suggestions, setSuggestions] = useState([]);
  const [action, setAction] = useState("");

  const handleSuggestions = async (value) => {
    const apiKey = process.env.REACT_APP_API_KEY;

    if (value.length >= 3) {
      try {
        const response = await axios.get(
          `https://api.openweathermap.org/geo/1.0/direct?q=${value}&limit=5&appid=${apiKey}`
        );
        const cities = response.data.map((city) => city.name);
        setSuggestions(cities);
      } catch (error) {
        console.error('Error fetching city data:', error);
      }
    } else {
      setSuggestions([]);
    }
  }

  const handleGetSubmit = (e) => {
    e.preventDefault();
    onGet(getCountryName);
  };

  const handlePostSubmit = (e) => {
    e.preventDefault();
    onPost(postCountryName);
  };

  const handleDeleteSubmit = (e) => {
    e.preventDefault();
    onDelete(deleteCountryName);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (action === "get") {
      handleGetSubmit(e);
    } else if (action === "post") {
      handlePostSubmit(e);
    } else if (action === "delete") {
      handleDeleteSubmit(e);
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
      
      <FormRow>
        <TextContainer>Get:</TextContainer>
        <InputWrapper>
          <InputForm
            value={getCountryName}
            onChange={(e) => {setGetCountryName(e.target.value);
                             handleSuggestions(getCountryName);
                            }}
            name="name"
            id="name"
            placeholder="City"
            autoComplete="off"
          />
          {suggestions.length > 0 && (
            <SelectForm
              onChange={(e) => handleSuggestions(getCountryName)}
              size={suggestions.length > 5 ? 5 : suggestions.length}
            >
              {suggestions.map((city, index) => (
                <option key={index} value={city}>
                  {city}
                </option>
              ))}
            </SelectForm>
          )}
        </InputWrapper>

        <ButtonRowButtonContainer>
          <ButtonContainer type="submit" onClick={() => setAction("get")}>
            Get city
          </ButtonContainer>
          <ButtonContainer type="button" onClick = { onCancel }>
            Cancel
          </ButtonContainer>
        </ButtonRowButtonContainer>
      </FormRow>

      
      <FormRow>
        <TextContainer>Post:</TextContainer>
        <InputWrapper>
          <InputForm
            value={postCountryName}
            onChange={(e) => setPostCountryName(e.target.value)}
            name="name"
            id="name"
            placeholder="City"
            autoComplete="off"
          />
        </InputWrapper>

        <ButtonRowButtonContainer>
          <ButtonContainer type="submit" onClick={() => setAction("post")}>
            Post city
          </ButtonContainer>
          <ButtonContainer type="button" onClick = { onCancel }>
            Cancel
          </ButtonContainer>
        </ButtonRowButtonContainer>
      </FormRow>

      <FormRow>
        <TextContainer>Delete:</TextContainer>
        <InputWrapper>
          <InputForm
            value={deleteCountryName}
            onChange={(e) => setDeleteCountryName(e.target.value)}
            name="name"
            id="name"
            placeholder="Id"
            autoComplete="off"
          />
        </InputWrapper>

        <ButtonRowButtonContainer>
          <ButtonContainer type="submit" onClick={() => setAction("delete")}>
            Delete city
          </ButtonContainer>
          <ButtonContainer type="button" onClick = { onCancel }>
            Cancel
          </ButtonContainer>
        </ButtonRowButtonContainer>
      </FormRow>

    </Form>
  );
};

export default SolarWatch;
