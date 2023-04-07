import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import NewsletterForm from "./NewsletterForm";
import FeaturedPostWidget from "./FeaturedPostWidget";

const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            <SearchForm />
            <CategoriesWidget />
            <FeaturedPostWidget />
            <NewsletterForm />
            <h1>Tag cloud</h1>
        </div>
    );
};

export default Sidebar;
