using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public class SimpleMergePolicy : MergePolicy
    {
        /// <summary>
        /// Static merge method, implementing the Simple merge policy from the Slice of Pie project description.
        /// Merges two documents and returns a copy of the latest document with the merged content.
        /// Associatéd unit tests is found in SliceOfPie_Testing.SimpleMergePolicyTest.cs
        /// </summary>
        /// <param name="original">
        /// The original document before changes</param>
        /// <param name="latest">The latest document with changes made</param>
        /// <returns>A copy of the document metadata along with the new content.</returns>
        protected override Document MergeDocuments(Document original, Document latest)
        {
            const string splitPattern = @"(?<=[.])";
            var splitter = new Regex(splitPattern);

            string[] originalArray = splitter.Split(original.Content.ToString()).Where(s => s != String.Empty).ToArray();
            string[] latestArray = splitter.Split(latest.Content.ToString()).Where(s => s != String.Empty).ToArray();
       
            var merged = new StringBuilder();

            int o = 0;
            int n = 0;
            int endOfO = originalArray.Length - 1;
            int endOfN = latestArray.Length - 1;

            // Always run once in case we can't split our document.
            do
            {
                // All remaining lines are new, we append from latest
                if (o == endOfO && n != endOfN)
                {
                    while (n <= endOfN)
                    {
                        merged.Append(latestArray[n++]);
                    }
                }
                
                // If two sentences are equal, just append one of them.
                else if (originalArray[o].Equals(latestArray[n]))
                {
                    merged.Append(originalArray[o]);
                    o++; n++;
                }

                // If we're at end of the latest, we assume the rest of the document has been removed.
                else if (o != endOfO && n == endOfN)
                {
                    o = endOfO;
                }                


                // If a sentence is different from original array we have to merge backwards.
                else if (!originalArray[o].Equals(latestArray[n]))
                {
                    Boolean isRemoved = true;
                    // t has a value different from 0 the original sentence is still found in the document.
                    int t = 0;

                    // we iterate through the rest of the document to find the original sentence.
                    for (int k = n + 1; k <= endOfN; k++)
                    {
                        if (originalArray[o].Equals(latestArray[k]))
                        {
                            // if we find it, flag it and remember the index.
                            isRemoved = false;
                            t = k;
                        }
                    }
                    // if the flag hasn't been sat, then the original sentence isn't here.
                    if (isRemoved)
                    {
                        o++;
                    }
                    else
                    {
                        // if the sentence still is in the document, append all lines from original place to new place in document.
                        for (int k = n; k < t; k++)
                        {
                            merged.Append(latestArray[k]);
                        }
                        // and continue iterating from that point on, merging rest of document.
                        n = t + 1; o++;
                    }
                }

            } while (o <= endOfO && n <= endOfN);
            
            // Since we use stringbuilder, we'll reuse the latest documents metadata.
            // Maybe change this later?
            latest.Content = merged.ToString();
            return latest;
        }

    }
}
