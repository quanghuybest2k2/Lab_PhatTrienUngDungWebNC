import { get_api, post_api, delete_api } from './Methods';

export function getTags() {
    return get_api(`https://localhost:44309/api/tags?PageSize=10&PageNumber=1`);
}

export async function getTagById(id = 0) {
    if (id > 0) {
        return get_api(`https://localhost:44309/api/tags/${id}`);
    } else {
        return null;
    }
}
export function addOrUpdateTag(formData) {
    return post_api('https://localhost:44309/api/tags', formData);
}

export async function deleteTagById(id) {
    if (id > 0) {
        return delete_api(`https://localhost:44309/api/tags/${id}`);
    } else {
        return null;
    }
}
