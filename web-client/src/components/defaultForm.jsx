import React, { Component } from "react";
import Joi from "joi-browser";

class DefaultForm extends Component {
  state = { data: {}, errors: {} };

  validate = () => {
    const options = {
      abortEarly: false,
    };
    const { error } = Joi.validate(this.state.data, this.schema, options);
    if (!error) return null;

    const errors = {};
    for (let item of error.details) {
      errors[item.path[0]] = item.message;
    }
    return errors;
  };

  validateProperty = ({ name, value }) => {
    const options = {};
    const obj = { [name]: value };
    const schema = { [name]: this.schema[name] };
    const { error } = Joi.validate(obj, schema, options);
    return error ? error.details[0].message : null;
  };

  handleSubmit = (e) => {
    e.preventDefault();
    const data = { ...this.state.data };
    Object.keys(data).map(
      (k) => typeof data[k] === "string" && (data[k] = data[k].trim())
    );

    this.setState({ data }, () => {
      const errors = this.validate();
      this.setState({ errors: errors || {} });
      if (errors) return;

      this.setState({ submitting: true });
      this.doSubmit();
    });
  };

  handleChange = ({ currentTarget: input }) => {
    const errors = { ...this.state.errors };
    const errorMessage = this.validateProperty(input);
    if (errorMessage) errors[input.name] = errorMessage;
    else delete errors[input.name];
    const data = { ...this.state.data };
    data[input.name] = input.value;
    this.setState({ data, errors });
  };

  renderButton(label, className = "", onClick = null) {
    const { submitting } = this.state;
    return submitting ? (
      <button disabled className={className || "btn btn-primary"}>
        Loading...
      </button>
    ) : !onClick ? (
      <button
        disabled={this.validate()}
        className={className || "btn btn-primary"}
      >
        {label}
      </button>
    ) : (
      <button onClick={onClick} className={className || "btn btn-primary"}>
        {label}
      </button>
    );
  }

  renderInput(
    name,
    label,
    type = "text",
    autoComplete = false,
    disabled = false
  ) {
    const { data, errors } = this.state;

    return (
      <DefaultInput
        type={type}
        name={name}
        value={data[name] || ""}
        label={label}
        onChange={this.handleChange}
        error={errors[name]}
        autoComplete={autoComplete ? null : "off"}
        disabled={disabled}
      />
    );
  }

  renderSelect(name, items, label, noEmpty = false, disabled = false) {
    const { data, errors } = this.state;
    return (
      <Select
        name={name}
        value={data[name] || ""}
        items={items}
        label={label}
        onChange={this.handleChange}
        error={errors[name]}
        noEmpty={noEmpty}
        disabled={disabled}
      />
    );
  }
}

export default DefaultForm;
