﻿namespace AspCore.CacheEntityClient.QueryContiner.Concrete
{
    public class MustQueryItemContainer : QueryItemContainer
    {
        public override BoolQueryContainer container { get; set; }

        public MustQueryItemContainer()
        {
            this.container = new BoolQueryContainer();
        }
    }
}