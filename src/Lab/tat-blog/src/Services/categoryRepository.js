import { get_api } from './Methods';

export async function getCategories() {
    return get_api(`https://localhost:44309/api/categories`);
}

export async function getCategoryById(id = 0) {
    if (id > 0) {
        return get_api(`https://localhost:44309/api/categories/${id}`);
    } else {
        return null;
    }
}
