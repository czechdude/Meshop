/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: CS
 */
jQuery.extend(jQuery.validator.messages, {
	required: "Tento údaj je povinný.",
	remote: "Prosím, opravte tento údaj.",
	email: "Prosím, zadejte platný e-mail.",
	url: "Prosím, zadejte platné URL.",
	date: "Prosím, zadejte platné datum.",
	dateISO: "Prosím, zadejte platné datum (ISO).",
	number: "Prosím, zadejte číslo.",
	digits: "Prosím, zadávejte pouze číslice.",
	creditcard: "Prosím, zadejte číslo kreditní karty.",
	equalTo: "Prosím, zadejte znovu stejnou hodnotu.",
	accept: "Prosím, zadejte soubor se správnou příponou.",
	maxlength: jQuery.validator.format("Prosím, zadejte nejvíce {0} znaků."),
	minlength: jQuery.validator.format("Prosím, zadejte nejméně {0} znaků."),
	rangelength: jQuery.validator.format("Prosím, zadejte od {0} do {1} znaků."),
	range: jQuery.validator.format("Prosím, zadejte hodnotu od {0} do {1}."),
	max: jQuery.validator.format("Prosím, zadejte hodnotu menší nebo rovnu {0}."),
	min: jQuery.validator.format("Prosím, zadejte hodnotu větší nebo rovnu {0}.")
});

jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?[\.\/-]\d\d?[\.\/-]\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    },
    range: function (value, element, param) {
		var globalizedValue = value.replace(",", ".");
		return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
	}
});