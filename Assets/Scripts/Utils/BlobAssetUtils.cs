// source: https://neilmarkcorre.wordpress.com/2020/12/01/working-with-scriptable-objects-and-blob-assets-and-creating-a-utility-class/

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public static class BlobAssetUtils {

    // We expose this to the clients to allow them to create BlobArray using BlobBuilderArray
    public static BlobBuilder BlobBuilder { get; private set; }

    // We allow the client to pass an action containing their blob creation logic
    public delegate void ActionRef<TBlobAssetType, in TDataType>(ref TBlobAssetType blobAsset, TDataType data);

    public static BlobAssetReference<TBlobAssetType> BuildBlobAsset<TBlobAssetType, TDataType>(TDataType data, ActionRef<TBlobAssetType, TDataType> action) where TBlobAssetType : struct {
        BlobBuilder = new BlobBuilder(Allocator.Temp);

        // Take note of the "ref" keywords. Unity will throw an error without them, since we're working with structs.
        ref TBlobAssetType blobAsset = ref BlobBuilder.ConstructRoot<TBlobAssetType>();

        // Invoke the client's blob asset creation logic
        action.Invoke(ref blobAsset, data);

        // Store the created reference to the memory location of the blob asset, before disposing the builder
        BlobAssetReference<TBlobAssetType> blobAssetReference = BlobBuilder.CreateBlobAssetReference<TBlobAssetType>(Allocator.Persistent);

        // We're not in a Using block, so we manually dispose the builder
        BlobBuilder.Dispose();

        // Return the created reference
        return blobAssetReference;
    }

    // source: https://coffeebraingames.wordpress.com/2020/11/29/getting-started-with-blob-asset/
    public static BlobAssetReference<T> CreateReference<T>(T value, Allocator allocator) where T : struct {
        BlobBuilder builder = new BlobBuilder(Allocator.TempJob);
        ref T data = ref builder.ConstructRoot<T>();
        data = value;
        BlobAssetReference<T> reference = builder.CreateBlobAssetReference<T>(allocator);
        builder.Dispose();

        return reference;
    }

}

public struct SampledAnimationCurveBlobAsset {
    public BlobArray<float> SampledAnimationCurve;
    public float Min;
    public float Max;

    /// <param name="time">Must be between Min and Max</param>
    public float Evaluate(float time) {
        int lenght = SampledAnimationCurve.Length - 1;
        time = math.clamp(time, Min, Max);
        float time01 = time / (Max - Min);
        float floatIndex = (time01 * lenght);
        int floorIndex = (int)math.floor(floatIndex);
        return SampledAnimationCurve[floorIndex];
    }
}

public struct SampledGradientBlobAsset {

    public BlobArray<float4> SampledGradient;

    /// <param name="time">Must be from 0 to 1</param>
    public float4 Evaluate(float time) {
        int length = SampledGradient.Length - 1;
        time = math.clamp(time, 0, 1);
        float floatIndex = (time * length);
        int floorIndex = (int)math.floor(floatIndex);
        return SampledGradient[floorIndex];

    }

}