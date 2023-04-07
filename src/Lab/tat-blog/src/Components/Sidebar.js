import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import NewsletterForm from "./NewsletterForm";
import FeaturedPostWidget from "./FeaturedPostWidget";
import RandomPostsWidget from "./RandomPostsWidget";
import BestAuthorsWidget from "./BestAuthorsWidget";
import TagCloudWidget from "./TagCloudWidget";
import ArchivesWidget from "./ArchivesWidget";

const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            <SearchForm />
            <CategoriesWidget />
            <FeaturedPostWidget />
            <RandomPostsWidget />
            <BestAuthorsWidget />
            <TagCloudWidget />
            <ArchivesWidget />
            <NewsletterForm />
        </div>
    );
};

export default Sidebar;
