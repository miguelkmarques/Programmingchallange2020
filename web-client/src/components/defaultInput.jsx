import React from "react";
import { Input, FormGroup, Label, FormFeedback } from "reactstrap";

const DefaultInput = ({ name, label, error, row, ...rest }) => {
  const restProp = { ...rest };
  if (!restProp.autoComplete) {
    delete restProp.autoComplete;
  }

  return (
    <FormGroup row={row}>
      <Label xs={row && "auto"} for={name}>
        {label}
      </Label>
      <Input
        xs={row && "auto"}
        {...restProp}
        name={name}
        id={name}
        invalid={error ? true : false}
        step="any"
      />
      <FormFeedback>{error}</FormFeedback>
    </FormGroup>
  );
};

export default DefaultInput;
