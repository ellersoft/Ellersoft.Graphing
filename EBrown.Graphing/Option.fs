module Option
let defaultValue value = function | Some v -> v | None -> value
