#!/bin/bash
for i in *Def.m4; do
    m4 $i > ../EffectDefs/"${i%.*}.cs"
done

for i in *Pane.m4; do
    m4 $i > ../EffectPanes/"${i%.*}.cs"
done

rm ../EffectDefs/ExampleDef.cs
rm ../EffectPanes/ExamplePane.cs
