function callHandlers(response, handlers, body) {
    if (handlers == null) return;

    switch (response.status) {
        case 200: // Ok
            if (handlers.ok != null) handlers.ok(response, body);
            break;
        case 201: // CreatedAt
            if (handlers.created != null) handlers.created(response, body);
            break;
        case 204: // NoContent
            if (handlers.empty != null) handlers.empty(response, body);
            break;
        case 400: // BadRequest
            if (handlers.badRequest != null) handlers.badRequest(response, body);
            break;
        case 404: // NotFound
            if (handlers.notFound != null) handlers.notFound(response, body);
            break;
    }

    if (response.status >= 200 && response.status < 300) {
        if (handlers.success != null) handlers.success(response, body);
    } else {
        if (handlers.error != null) handlers.error(response, body);
    }
}

export async function fetchGet(path, handlers) {
    if (handlers == null) return;

    let response = await fetch(path);
    let body = null;

    try {
        body = await response.json();
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}

export async function fetchGetId(path, id, handlers) {
    if (handlers == null) return;

    let response = await fetch(`${path}/${id}`);
    let body = null;

    try {
        body = await response.json();
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}

export async function fetchGetIdBool(path, id, handlers) {
    if (handlers == null) return;

    let response = await fetch(`${path}/${id}`);
    let body = false;

    try {
        body = (await response.text()) == "true";
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}

export async function fetchPut(path, id, value, handlers) {
    let response = await fetch(`${path}/${id}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(value)
    });

    let body = null;

    try {
        body = await response.json();
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}

export async function fetchPost(path, value, handlers) {
    let response = await fetch(path, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(value)
    });

    let body = null;

    try {
        body = await response.json();
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}

export async function fetchDelete(path, id, handlers) {
    let response = await fetch(`${path}/${id}`, {
        method: "DELETE",
    });

    let body = null;

    try {
        body = await response.json();
    } catch (_) {

    }

    callHandlers(response, handlers, body);
}