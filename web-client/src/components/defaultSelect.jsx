import React from "react";
import { Input, FormGroup, Label, FormFeedback } from "reactstrap";

const DefaultSelect = ({ name, items, label, error, noEmpty, ...rest }) => {
  return (
    <FormGroup>
      <Label for={name}>{label}</Label>
      <Input
        {...rest}
        type="select"
        id={name}
        name={name}
        invalid={error ? true : false}
      >
        {!noEmpty && <option value="" />}
        {items.map((item) => (
          <option key={item["id"]} value={item["id"]}>
            {item["name"]}
          </option>
        ))}
      </Input>
      <FormFeedback>{error}</FormFeedback>
    </FormGroup>
  );
};

export default DefaultSelect;
